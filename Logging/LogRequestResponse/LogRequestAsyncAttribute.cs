
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using Logging.Models;
using System.Web.Mvc;

namespace Logging.LogRequestResponse
{
    public class LogRequestAsyncAttribute : TypeFilterAttribute
    {
        public LogRequestAsyncAttribute() : base(typeof(LogRequestAsyncImp))
        {
        }

        private class LogRequestAsyncImp : Attribute, IAsyncActionFilter, IFilterMetadata
        {
            private readonly ILogger<LogRequestAsyncImp> _logger;
            private string message;
            private string messageConstructed;


            public LogRequestAsyncImp(ILogger<LogRequestAsyncImp> logger)
            {
                _logger = logger;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                //GetMethodName(context); 
                HttpContext httpContext = context.HttpContext;
                string eventID = Guid.NewGuid().ToString();

                var isIgnored = context.ActionDescriptor.EndpointMetadata.OfType<IgnoreLogging>().Any();
                var ignoreLoggingItems = context.ActionDescriptor.EndpointMetadata.OfType<IgnoreLoggingItems>().FirstOrDefault();
                var includeLoggingItems = context.ActionDescriptor.EndpointMetadata.OfType<IncludeLoggingItems>().FirstOrDefault();
                var ignoredItems = ignoreLoggingItems != null ? ignoreLoggingItems.IngoredItems : Array.Empty<Ignore>();
                var includedItems = includeLoggingItems != null ? includeLoggingItems.IncludedItems : Array.Empty<Include>();
                var startTime = DateTime.UtcNow;
                var serviceName = httpContext.Request.Host.ToString() + httpContext.Request.Path.ToString();
                string body = "", header = "";
                if (!((IEnumerable<Ignore>)ignoredItems).Contains(Ignore.RequestBody))
                {
                    body = await ReadRequestBody(httpContext.Request);
                    PushLogContextProperty(LogContextProperties.REQUEST_BODY, body.ToString());
                }

                if (!ignoredItems.Contains(Ignore.RequestHeaders))
                {
                    header = FormatHeaders(httpContext.Request.Headers);
                    PushLogContextProperty(LogContextProperties.REQUEST_HEADER, header);
                }

                PushLogContextProperty(LogContextProperties.ACTION_ARGS, context.ActionArguments);
                PushLogContextProperty(LogContextProperties.HTTPMETHOD, httpContext.Request.Method);
                _logger.LogInformation($"Request Method: {httpContext.Request.Method}", serviceName);
                PushLogContextProperty(LogContextProperties.REQUEST_QUERYSTRING, httpContext.Request.QueryString);
                _logger.LogInformation($"Request QueryString: {httpContext.Request.QueryString}", serviceName);
                _logger.LogInformation($"Request Header: {header}");
                _logger.LogInformation($"Request Body: {body}");

                PushLogContextProperty(LogContextProperties.SCHEMA, httpContext.Request.Scheme);

                PushLogContextProperty(LogContextProperties.HOST, httpContext.Request.Host);
                PushLogContextProperty(LogContextProperties.HOST_REMOTE, httpContext.Features.Get<IServerVariablesFeature>()?["REMOTE_HOST"]?.ToString());

                PushLogContextProperty(LogContextProperties.USERNAME, httpContext.User.Identity.IsAuthenticated ? httpContext.GetClaimValue(TPSClaimNames.Username) : "anonymous");


                PushLogContextProperty(LogContextProperties.IPADDRESS_LOCAL, httpContext.Connection.LocalIpAddress.ToString());
                PushLogContextProperty(LogContextProperties.IPADDRESS_REMOTE, httpContext.Connection.RemoteIpAddress.ToString());
                PushLogContextProperty(LogContextProperties.IPADDRESS_REMOTE_GATEWAY, context.HttpContext.Request.Headers["remote-host"].ToString());

                PushLogContextProperty(LogContextProperties.PAGE, context.HttpContext.Request.Headers["page"].ToString());
                PushLogContextProperty(LogContextProperties.SYSTEM, context.HttpContext.Request.Headers["system"].ToString());
                PushLogContextProperty(LogContextProperties.MODULE, context.HttpContext.Request.Headers["module"].ToString());

                ActionExecutedContext resultContext;

                if (!context.ModelState.IsValid)
                {
                    string elapsed = string.Format("{000}", (DateTime.UtcNow - startTime).TotalMilliseconds);

                    PushLogContextProperty(LogContextProperties.LOGTYPE, LogType.ModelErrors.ToString());
                    PushLogContextProperty(LogContextProperties.MODELERRORS, JsonConvert.SerializeObject(GetModelErrors(context, eventID)));
                    PushLogContextProperty(LogContextProperties.ELAPSED, elapsed);

                    // add message
                    message = $"Model Errors: Invalid data in method {serviceName}";
                    messageConstructed = "Model Errors: Invalid data in method: {serviceName}";

                    PushLogContextProperty(LogContextProperties.MESSAGE, message);
                    PushLogContextProperty(LogContextProperties.EVENTID, eventID);

                    _logger.LogError(messageConstructed, serviceName);
                }
                else
                {
                    if (httpContext.Response.StatusCode != 401)
                    {
                        // perform the action
                        resultContext = await next();
                        int? statusCode = 200;

                        if (resultContext.Result is ObjectResult)
                        {
                            statusCode = ((ObjectResult)resultContext.Result).StatusCode;
                        }
                        else if (resultContext.Result is ViewResult)
                        {
                            statusCode = ((ViewResult)resultContext.Result).StatusCode;
                        }

                        // check if exceptions had happened
                        if (resultContext.Exception != null)
                        {
                            await HandleExceptionsAsync(context, startTime, serviceName, resultContext, eventID, resultContext);
                        }
                        else if (isIgnored)
                        {
                            // do nothing - don't log
                        }
                        else if (resultContext.HttpContext.Response.StatusCode != 200 && resultContext.HttpContext.Response.StatusCode != 201 ||
                            statusCode != 200 && statusCode != 201 && statusCode != null)
                        {
                            await HandleExceptionsAsync(context, startTime, serviceName, resultContext, eventID, resultContext);
                        }
                        else
                        {
                            //if (!((IEnumerable<Ignore>)ignoredItems).Contains(Ignore.ResponseBody))
                            //{
                            //	PushLogContextProperty(RESPONSE_BODY, await GetResponseBody(resultContext.Result));
                            //}

                            //if (includedItems.Contains(Include.ResponseBody))
                            //{
                                var response = await GetResponseBody(resultContext.Result);
                                PushLogContextProperty(LogContextProperties.RESPONSE_BODY, response);
                                _logger.LogInformation($"Response: {response}");
                            //}

                            string elapsed = string.Format("{000}", (DateTime.UtcNow - startTime).TotalMilliseconds);

                            PushLogContextProperty(LogContextProperties.STATUSCODE, resultContext.HttpContext.Response.StatusCode);
                            PushLogContextProperty(LogContextProperties.ELAPSED, elapsed);
                            PushLogContextProperty(LogContextProperties.LOGTYPE, LogType.NormalCall.ToString());

                            // add message
                            message = $"Normal Call: {serviceName} was called";
                            messageConstructed = "Normal Call: Method: {serviceName} was called - {Elapsed} ms";

                            PushLogContextProperty(LogContextProperties.MESSAGE, message);

                            _logger.LogInformation(messageConstructed, serviceName, elapsed);

                        }
                    }

                }
            }

            private void PushLogContextProperty(string name, object value)
            {
                LogContext.PushProperty(name, value);
            }
            private async Task HandleExceptionsAsync(ActionExecutingContext context, DateTime startTime, string serviceName, ActionExecutedContext resultContext, string eventID, ActionExecutedContext executedContext)
            {
                var exception = resultContext.Exception;

                string elapsed = string.Format("{000}", (DateTime.UtcNow - startTime).TotalMilliseconds);

                PushLogContextProperty(LogContextProperties.ELAPSED, elapsed);
                PushLogContextProperty(LogContextProperties.LOGTYPE, LogType.ExceptionHandled.ToString());

                var responseBody = await GetResponseBody(resultContext.Result);
                if (!string.IsNullOrEmpty(responseBody))
                {
                    PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                }

                //PushLogContextProperty("Response.Body2", await Task.Run(() => JsonConvert.SerializeObject(((ResponseException)((Microsoft.AspNetCore.Mvc.ObjectResult)resultContext.Result).Value).Response)));
                //PushLogContextProperty("Response.Body2", await GetResponseBody((ObjectResult)resultContext.Result));

                // add message
                var exMessage = resultContext.Exception?.Message;
                message = $"Handled Exception: {exMessage} in method: {serviceName}";
                messageConstructed = resultContext.Exception != null ? "Handled Exception: {exMessage} in method: {serviceName}" : "Handled Exception{exMessage} in method: {serviceName}";

                PushLogContextProperty(LogContextProperties.MESSAGE, message);
                PushLogContextProperty(LogContextProperties.EVENTID, eventID);

                _logger.LogError(resultContext.Exception, messageConstructed, exMessage == null ? string.Empty : exMessage, serviceName);

                resultContext.Exception = null;

                switch (exception)
                {
                    case ResponseException:
                        var responseException = ((ResponseException)exception).Response;
                        responseException.Method = context.HttpContext.Request.Method;
                        responseException.EventID = eventID;
                        //response.Message = exception.Message;
                        responseException.Status = 400;

                        resultContext.Result = new ObjectResult(responseException)
                        {
                            StatusCode = responseException.Status
                        };
                        break;
                    case ModelErrorException:
                        var modelErrorException = GetApiResponse(null, exception?.Message, 422, true, eventID, context.HttpContext.Request.Method,
                                                                    new Error
                                                                    {
                                                                        Location = ErrorLocation.controls,
                                                                        Controls = new List<ErrorControl>
                                                                        {
                                                                            new ErrorControl() { PropertyName = ((ModelErrorException)exception).PropertyName,   Error = ((ModelErrorException)exception).Message }
                                                                        }
                                                                    });
                        if (string.IsNullOrEmpty(responseBody))
                        {
                            responseBody = JsonConvert.SerializeObject(modelErrorException);
                            PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                        }
                        resultContext.Result = new UnprocessableEntityObjectResult(modelErrorException);
                        break;
                    case NotFoundException:
                        var notFoundException = GetApiResponse(null, exception?.Message, 404, true, eventID, context.HttpContext.Request.Method,
                                                                    new Error
                                                                    {
                                                                        Location = ErrorLocation.form,
                                                                    });
                        if (string.IsNullOrEmpty(responseBody))
                        {
                            responseBody = JsonConvert.SerializeObject(notFoundException);
                            PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                        }
                        resultContext.Result = new NotFoundObjectResult(notFoundException);
                        break;
                    default:
                        Type contextType = executedContext?.Result?.GetType();
                        Type contextValueType = null;
                        IActionResult contextResult = null;

                        if (contextType == typeof(ObjectResult))
                        {
                            contextResult = (ObjectResult)executedContext?.Result;
                            contextValueType = ((ObjectResult)executedContext?.Result)?.Value.GetType();
                        }
                        else if (contextType == typeof(ViewResult))
                        {
                            contextResult = (ViewResult)executedContext?.Result;
                        }

                        if (contextType != null && contextValueType != null && contextType == typeof(ObjectResult) && contextValueType == typeof(ResponseException))
                        {
                            var defaultResponseException = ((ResponseException)((ObjectResult)executedContext.Result).Value).Response;
                            defaultResponseException.EventID = eventID;

                            if (string.IsNullOrEmpty(responseBody))
                            {
                                responseBody = JsonConvert.SerializeObject(defaultResponseException);
                                PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                            }
                            resultContext.Result = new BadRequestObjectResult(defaultResponseException);
                        }
                        else if (contextType != null && contextValueType != null && contextType == typeof(ObjectResult) && contextValueType == typeof(ApiResponse))
                        {
                            var defaultApiResponse = (ApiResponse)((ObjectResult)executedContext.Result).Value;
                            defaultApiResponse.EventID = eventID;

                            if (string.IsNullOrEmpty(responseBody))
                            {
                                responseBody = JsonConvert.SerializeObject(defaultApiResponse);
                                PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                            }

                            switch (defaultApiResponse.Status)
                            {
                                case 400:
                                    resultContext.Result = new BadRequestObjectResult(defaultApiResponse);
                                    break;
                                case 404:
                                    resultContext.Result = new NotFoundObjectResult(defaultApiResponse);
                                    break;
                                default:
                                    resultContext.Result = new BadRequestObjectResult(defaultApiResponse);
                                    break;
                            }
                        }
                        else
                        {
                            var badRequestObjectResult = GetApiResponse(null, exception?.Message, 400, true, eventID, context.HttpContext.Request.Method,
                                                                        new Error
                                                                        {
                                                                            Location = ErrorLocation.form,
                                                                        });

                            if (string.IsNullOrEmpty(responseBody))
                            {
                                responseBody = JsonConvert.SerializeObject(badRequestObjectResult);
                                PushLogContextProperty(LogContextProperties.RESPONSE_BODY, responseBody);
                            }
                            //resultContext.Result = new BadRequestObjectResult(((ObjectResult)executedContext?.Result)?.Value);
                            resultContext.Result = new BadRequestObjectResult(badRequestObjectResult);
                        }

                        break;
                }
            }

            private ApiResponse GetApiResponse(object data, string message, int status, bool isError, string eventID, string responseMethod, Error error)
            {
                return new ApiResponse()
                {
                    Data = data,
                    Message = message,
                    Status = status,
                    IsError = isError,
                    EventID = eventID,
                    Method = responseMethod,
                    Error = error
                };
            }

            private List<ValidationError> GetModelErrors(ActionExecutingContext context, string eventID)
            {
                context.ModelState.Values.SelectMany(v => v.Errors);
                List<ValidationError> list = context.ModelState.Keys.SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key.Replace("$.", ""), x.ErrorMessage))).ToList();

                ModelStateDictionary.ValueEnumerable values = context.ModelState.Values;

                var badRequestObjectResult = GetApiResponse(null, "Unprocessable, Validation Failed", 422, true, eventID, context.HttpContext.Request.Method,
                                                                    new Error
                                                                    {
                                                                        Location = ErrorLocation.controls,
                                                                        Controls = list.Select(x => new ErrorControl() { Error = x.Message, PropertyName = x.Field }).ToList()
                                                                    });

                context.Result = new BadRequestObjectResult(badRequestObjectResult);

                return list;
            }

            private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => "{" + kvp.Key + ": " + string.Join(", ", kvp.Value) + "}"));

            private static async Task<string> GetResponseBody(IActionResult result)
            {
                object responseBody = null;
                string returnResult = string.Empty;

                try
                {
                    switch (result)
                    {
                        case OkObjectResult okObjectResult:
                            responseBody = okObjectResult.Value;
                            break;
                        case JsonResult jsonResult:
                            responseBody = jsonResult.Value;
                            break;
                        case ViewResult viewResult:
                            responseBody = viewResult.ViewData;
                            string viewName = viewResult.ViewName;
                            break;
                        case ObjectResult objectResult:
                            if (objectResult.Value.GetType() == typeof(ResponseException))
                            {
                                responseBody = ((ResponseException)objectResult.Value)?.Response;
                            }
                            else if (objectResult.Value.GetType() == typeof(ApiResponse))
                            {
                                responseBody = (ApiResponse)objectResult.Value;
                            }
                            break;
                    }

                    if (responseBody != null)
                        returnResult = await Task.Run(() => JsonConvert.SerializeObject(responseBody));
                }
                catch { }

                return returnResult;
            }

            private static async Task<string> ReadRequestBody(HttpRequest request)
            {
                string str = "";
                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] buf = new byte[1024];
                   var count = stream.Read(buf, 0, 1024);
                    if (stream.CanRead && count > 0)
                    {
                        request.Body.Seek(0L, SeekOrigin.Begin);
                        await request.Body.CopyToAsync(stream);
                        str = await Task.Run(() => Encoding.UTF8.GetString(stream.ToArray()));
                        request.Body.Seek(0L, SeekOrigin.Begin);

                    }
                  
                }
                return str;
            }
        }
    }
}
