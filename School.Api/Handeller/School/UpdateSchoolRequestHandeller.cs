using Common.Service.IServices;
using MediatR;
using School.Api.Command.School;

namespace School.Api.Handeller.School
{
    public class UpdateSchoolRequestHandeller : IRequestHandler<CreateSchoolRequest>
    {
        private readonly IBaseService<Domain.Models.School> _schoolAppService;

        public UpdateSchoolRequestHandeller(IBaseService<Domain.Models.School> schoolAppService)
        {
            _schoolAppService = schoolAppService;
        }
        Task<Unit> IRequestHandler<CreateSchoolRequest, Unit>.Handle(CreateSchoolRequest request, CancellationToken cancellationToken)
        {
            _schoolAppService.Update(request.CreateOrUpdateSchool);
            return Task.FromResult(Unit.Value);

        }
    }
}
