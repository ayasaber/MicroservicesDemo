using MediatR;

namespace School.Api.Query.School
{
    public class GetSchoolByIdQuery : IRequest<Domain.Models.School>
    {
        public int Id { get; }
        public GetSchoolByIdQuery(int id)
        {
            Id = id;
        }
    }
}
