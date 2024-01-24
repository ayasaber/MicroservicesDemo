using Common.Service.IServices;
using MediatR;
using School.Api.Query.School;

namespace School.Api.Handeller.School
{
    public class GetSchoolByIdQueryHandeler : IRequestHandler<GetSchoolByIdQuery, Domain.Models.School>
    {
        private readonly IBaseService<Domain.Models.School> _schoolAppService;
        public GetSchoolByIdQueryHandeler(IBaseService<Domain.Models.School> schoolAppService)
        {
                _schoolAppService = schoolAppService;
        }
        public async Task<Domain.Models.School> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _schoolAppService.Get(request.Id);
            return result;
        }
    }
}
