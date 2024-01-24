using Common.Service.IServices;
using MediatR;
using Student.Api.Command.Student;

namespace Student.Api.Handeller.Student
{
    public class UpdateStudentRequestHandeller : IRequestHandler<CreateStudentRequest>
    {
        private readonly IBaseService<Domain.Models.Student> _StudentAppService;

        public UpdateStudentRequestHandeller(IBaseService<Domain.Models.Student> studentAppService)
        {
            _StudentAppService = studentAppService;
        }
        public async Task<Unit> Handle(CreateStudentRequest request, CancellationToken cancellationToken)
        {
            _StudentAppService.Update(request.CreateOrUpdateSchool);
            return Unit.Value;
        }
    }
}
