using Common.Service.IServices;
using MediatR;
using Student.Api.Command.Student;

namespace Student.Api.Handeller.Student
{
    public class CreateStudentRequestHandeller : IRequestHandler<CreateStudentRequest>
    {
        private readonly IBaseService<Domain.Models.Student> _schoolAppService;
        public CreateStudentRequestHandeller(IBaseService<Domain.Models.Student> schoolAppService)
        {
            _schoolAppService = schoolAppService;
        }
       async Task<Unit> IRequestHandler<CreateStudentRequest, Unit>.Handle(CreateStudentRequest request, CancellationToken cancellationToken)
        {
            _schoolAppService.Insert(request.CreateOrUpdateSchool);
            return Unit.Value;
        }
    }
}
