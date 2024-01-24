using MediatR;

namespace School.Api.Command.School
{
    public class DeletSchoolRequest : IRequest<bool>
    {
        public Domain.Models.School School { get; }

        public DeletSchoolRequest(Domain.Models.School school)
        {
            School = school;
        }
    }
}
