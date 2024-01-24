using Common.Service.IServices;
using MediatR;
using School.Api.Command.School;

namespace School.Api.Handeller.School
{
    public class DeletSchoolRequestHandeler : IRequestHandler<DeletSchoolRequest, bool>
    {
        private readonly IBaseService<Domain.Models.School> _schoolAppService ;

        public DeletSchoolRequestHandeler(IBaseService<Domain.Models.School> schoolAppService)
        {
                _schoolAppService = schoolAppService;
        }
        public async Task<bool> Handle(DeletSchoolRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _schoolAppService.Delete(request.School);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
         }
    }
}
