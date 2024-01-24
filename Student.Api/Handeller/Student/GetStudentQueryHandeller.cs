using Common.Service.IServices;
using MediatR;
using Student.Api.Query.Student;

namespace Student.Api.Handeller.Student
{
    public class GetStudentQueryHandeller : IRequestHandler<GetStudentQuery, IEnumerable<Domain.Models.Student>>
    {
        private readonly IBaseService<Domain.Models.Student> _StudentAppService;
        public GetStudentQueryHandeller(IBaseService<Domain.Models.Student> studentAppService)
        {
            _StudentAppService = studentAppService;
        }

        public async Task<IEnumerable<Domain.Models.Student>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var result = _StudentAppService.GetAll();
            return result;
        }
    }
}
