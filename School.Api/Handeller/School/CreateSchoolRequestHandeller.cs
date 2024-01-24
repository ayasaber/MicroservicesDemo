using Common.Service.IServices;
using MediatR;
using School.Api.Command.School;

namespace School.Api.Handeller.School
{
    public class CreateSchoolRequestHandeller : IRequestHandler<CreateSchoolRequest>
    {
        private readonly IBaseService<Domain.Models.School> _schoolAppService;
        public CreateSchoolRequestHandeller(IBaseService<Domain.Models.School> schoolAppService)
        {
                _schoolAppService = schoolAppService;
        }
      
       async Task<Unit> IRequestHandler<CreateSchoolRequest, Unit>.Handle(CreateSchoolRequest request, CancellationToken cancellationToken)
        {
            _schoolAppService.Insert(request.CreateOrUpdateSchool);
            return Unit.Value;
        }
    }
}
