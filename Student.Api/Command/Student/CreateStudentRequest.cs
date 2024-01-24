using MediatR;

namespace Student.Api.Command.Student
{
    public class CreateStudentRequest : IRequest
    {
        public Domain.Models.Student CreateOrUpdateSchool { get; }
        public CreateStudentRequest(Domain.Models.Student createOrUpdateSchool)
        {
            CreateOrUpdateSchool = createOrUpdateSchool;
        }
    }
}
