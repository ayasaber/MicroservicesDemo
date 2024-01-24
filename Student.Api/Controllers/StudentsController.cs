using Student.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using Common.Service.IServices;
using MediatR;
using Student.Api.Query.Student;
using Student.Api.Command.Student;

namespace Students.Api.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IBaseService<Student.Domain.Models.Student> _baseService;
        private readonly StudentDbContext _studentDbContext;
        private readonly IMediator _mediator;
        public StudentsController(IBaseService<Student.Domain.Models.Student> baseService, StudentDbContext studentDbContext, IMediator mediator)
        {
            _baseService = baseService;
            _studentDbContext = studentDbContext;
            _mediator = mediator;
        }
        [HttpGet(nameof(GetStudentById))]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            var query = new GetStudentByIdQuery(Id);
            var obj = await _mediator.Send(query);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }
        [HttpGet(nameof(GetAllStudentAsync))]
        public async Task<IActionResult> GetAllStudentAsync()
        {
            var query = new GetStudentQuery();
            var obj = await _mediator.Send(query);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(obj);
            }
        }
        [HttpPost(nameof(CreateStudentAsync))]
        public async Task<IActionResult> CreateStudentAsync(Student.Domain.Models.Student student)
        {
            if (student != null)
            {
                var query = new CreateStudentRequest(student);
                var obj = await _mediator.Send(query);
                return Ok("Created Successfully");
            }
            else
            {
                return BadRequest("Somethingwent wrong");
            }
        }
        [HttpPost(nameof(UpdateStudentAsync))]
        public async Task<IActionResult> UpdateStudentAsync(Student.Domain.Models.Student student)
        {
            if (student != null)
            {
                var query = new UpdateStudentRequest(student);
                var obj = await _mediator.Send(query);
                return Ok("Updated SuccessFully");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete(nameof(DeleteStudentAsync))]
        public async Task<IActionResult> DeleteStudentAsync(Student.Domain.Models.Student student)
        {
            if (student != null)
            {
                var query = new DeletStudentRequest(student);
                var obj = await _mediator.Send(query);
                return Ok("Deleted Successfully");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

    }
}