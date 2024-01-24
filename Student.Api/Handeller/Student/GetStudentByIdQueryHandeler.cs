using Common.Service.IServices;
using MediatR;
using Student.Api.Query.Student;

namespace Student.Api.Handeller.Student
{
    public class GetStudentByIdQueryHandeler : IRequestHandler<GetStudentByIdQuery, Domain.Models.Student>
    {
        private readonly IBaseService<Domain.Models.Student> _StudentAppService;
        public GetStudentByIdQueryHandeler(IBaseService<Domain.Models.Student> studentAppService)
        {
            _StudentAppService = studentAppService;
        }
        public  async Task<Domain.Models.Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _StudentAppService.Get(request.Id);
            return result;
        }
    }
}
