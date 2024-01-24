using MediatR;

namespace Student.Api.Query.Student
{
    public class GetStudentQuery : IRequest<IEnumerable<Domain.Models.Student>>
    {
    }
}
