using MediatR;

namespace Student.Api.Command.Student
{
    public class UpdateStudentRequest : IRequest<Domain.Models.Student>
    {
        public Domain.Models.Student CreateOrUpdateStudent { get; }
        public UpdateStudentRequest(Domain.Models.Student createOrUpdateStudent)
        {
            CreateOrUpdateStudent = createOrUpdateStudent;
        }
    }
}
