using MediatR;

namespace School.Api.Query.School
{
    public class GetSchoolQuery : IRequest<IEnumerable<Domain.Models.School>>
    {
    }
}
