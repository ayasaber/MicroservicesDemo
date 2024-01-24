using Common.Service.IServices;
using MediatR;
using School.Api.Query.School;

namespace School.Api.Handeller.School
{
    public class GetSchoolQueryHandeller : IRequestHandler<GetSchoolQuery, IEnumerable<Domain.Models.School>>
    {
        private readonly IBaseService<Domain.Models.School> _schoolAppService;

        public GetSchoolQueryHandeller(IBaseService<Domain.Models.School> schoolAppService)
        {
            _schoolAppService = schoolAppService;
        }
        public async Task<IEnumerable<Domain.Models.School>> Handle(GetSchoolQuery request, CancellationToken cancellationToken)
        {
            var result = _schoolAppService.GetAll();
            return result;
        }
    }
}
