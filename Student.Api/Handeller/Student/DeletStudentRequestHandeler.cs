using Common.Service.IServices;
using MediatR;
using Student.Api.Command.Student;

namespace Student.Api.Handeller.Student
{
    public class DeletStudentRequestHandeler : IRequestHandler<DeletStudentRequest, bool>
    {
        private readonly IBaseService<Domain.Models.Student> _studentAppService;

        public DeletStudentRequestHandeler(IBaseService<Domain.Models.Student> studentAppService)
        {
            _studentAppService = studentAppService;
        }
        public async Task<bool> Handle(DeletStudentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _studentAppService.Delete(request.Student);
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
