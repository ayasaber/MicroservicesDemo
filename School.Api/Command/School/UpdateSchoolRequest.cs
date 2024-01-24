using MediatR;

namespace School.Api.Command.School
{
    public class UpdateSchoolRequest : IRequest<Domain.Models.School>
    {
        public Domain.Models.School CreateOrUpdateSchool { get; }
        public UpdateSchoolRequest(Domain.Models.School createOrUpdateSchool)
        {
            CreateOrUpdateSchool = createOrUpdateSchool;
        }
    }
}
