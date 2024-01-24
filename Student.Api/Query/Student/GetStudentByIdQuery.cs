using MediatR;

namespace Student.Api.Query.Student
{
    public class GetStudentByIdQuery : IRequest<Domain.Models.Student>
    {
        public int Id { get; }
        public GetStudentByIdQuery(int id)
        {
            Id = id;
        }
    }
}
