using School.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using Common.Service.IServices;
using Grpc.Net.Client;
using Student.Api.Protos;
using Grpc.Core;
using MediatR;
using School.Api.Query.School;
using School.Api.Command.School;

namespace Schools.Api.Controllers
{
	[Route("api/Schools")]
	[ApiController]
	public class SchoolController : ControllerBase
	{
		private readonly IBaseService<School.Domain.Models.School> _baseService;
		private readonly SchoolDbContext _SchoolDbContext;

        private readonly IMediator _mediator;
     
        public SchoolController(IBaseService<School.Domain.Models.School> customService, SchoolDbContext SchoolDbContext, IMediator mediator)
		{
			_baseService = customService;
			_SchoolDbContext = SchoolDbContext;
            _mediator = mediator;

        }

        [HttpGet(nameof(GetAllSchoolAsync))]
		public async Task<IActionResult> GetAllSchoolAsync()
		{
            var query = new GetSchoolQuery();
            var obj = await _mediator.Send(query);
 			if (obj == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(obj);
			}
		}
		[HttpGet(nameof(GetAllSchoolByFilter))]
		public IActionResult GetAllSchoolByFilter(string addres="Alex")
		{
			var obj = _baseService.GetAll(s=>s.Address== addres);
			if (obj == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(obj);
			}
		}
		
		[HttpGet(nameof(GetSchoolByIdAsync))]
		public async Task<IActionResult> GetSchoolByIdAsync(int Id)
		{
            var query = new GetSchoolByIdQuery(Id);
            var obj = await _mediator.Send(query);
 			if (obj == null)
			{
				return NotFound();
			}
			else
			{
				return Ok(obj);
			}
		}
		
		[HttpPost(nameof(CreateSchoolAsync))]
		public async Task<IActionResult> CreateSchoolAsync(School.Domain.Models.School School)
		{
           
            if (School != null)
			{
                var query = new CreateSchoolRequest(School);
                var obj = await _mediator.Send(query);
                return Ok("Created Successfully");
			}
			else
			{
				return BadRequest("Somethingwent wrong");
			}
		}
		[HttpPost(nameof(UpdateSchoolAsync))]
		public async Task<IActionResult> UpdateSchoolAsync(School.Domain.Models.School School)
		{
			if (School != null)
			{
                var query = new UpdateSchoolRequest(School);
				var obj = await _mediator.Send(query);
                return Ok("Updated SuccessFully");
			}
			else
			{
				return BadRequest();
			}
		}
		[HttpDelete(nameof(DeleteSchoolAsync))]
		public async Task<IActionResult> DeleteSchoolAsync(School.Domain.Models.School school)
		{
			if (school != null)
			{
                var query = new DeletSchoolRequest(school);
				var obj = await _mediator.Send(query);
                return Ok("Deleted Successfully");
			}
			else
			{
				return BadRequest("Something went wrong");
			}
		}

		// Call the remote student microservice to get students  who've applied to the school

		[HttpGet(nameof(GetStudentsBySchoolId))]
		public IActionResult GetStudentsBySchoolId(int schoolId)
		{
			try
			{
				var channel = GrpcChannel.ForAddress("https://localhost:7256");
				var studentClient = new StudentContract.StudentContractClient(channel);

				var request = new GetStudentsBySchoolIdRequest { SchoolId = schoolId };

				var response = studentClient.GetStudentsBySchoolId(request);

				// Check if the gRPC call was successful
				if (response != null && response != null)
				{
					return Ok(response); 
				}
				else
				{
					return NotFound("No students found for the given school ID."); 
				}
			}
			// Handle gRPC-specific exceptions
			catch (RpcException ex)
			{
				Console.WriteLine($"Error communicating with Student microservice: {ex.Status}");
				return StatusCode(500, "Internal Server Error"); 
			}
			// Handle other exceptions
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return StatusCode(500, "Internal Server Error"); 
			}
		}

	}
}