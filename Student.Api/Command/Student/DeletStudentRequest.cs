using MediatR;

namespace Student.Api.Command.Student
{
    public class DeletStudentRequest : IRequest<bool>
    {
        public Domain.Models.Student Student { get; }
        public DeletStudentRequest(Domain.Models.Student student)
        {
            Student = student;
        }
    }
}
