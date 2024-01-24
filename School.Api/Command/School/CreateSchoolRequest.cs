using MediatR;

namespace School.Api.Command.School
{
    public class CreateSchoolRequest : IRequest
    {
        public Domain.Models.School CreateOrUpdateSchool { get; }
        public CreateSchoolRequest(Domain.Models.School createOrUpdateSchool)
        {
            CreateOrUpdateSchool = createOrUpdateSchool;
        }
    }
}
