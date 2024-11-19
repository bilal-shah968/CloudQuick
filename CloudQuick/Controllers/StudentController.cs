using AutoMapper;
using CloudQuick.Data;
using CloudQuick.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CloudQuick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CloudDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentController(ILogger<StudentController> logger, CloudDbContext dbContext,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper =  mapper;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTo>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudents method started");
             var students = await _dbContext.Students.ToListAsync();

            var studentDToData = _mapper.Map<List<StudentDTo>>(students);
            
            //OK - 200 - Success
            return Ok(studentDToData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTo>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request: Invalid ID.");
                return BadRequest("Invalid ID provided.");
            }

            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                _logger.LogError($"Student with ID {id} not found.");
                return NotFound($"Student with ID {id} not found.");
            }

            var studentDto = _mapper.Map<StudentDTo>(student);
            //Ok - 200 - Success
            return Ok(studentDto);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        public async Task<ActionResult<StudentDTo>> GetStudentByNameAsync(string name)
        {
            _logger.LogInformation($"Searching for student with name: {name}");

            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentName == name);
            if (student == null)
            {
                _logger.LogWarning($"Student with name '{name}' not found.");
                return NotFound($"Student with name '{name}' not found.");
            }

            var studentDto = _mapper.Map<StudentDTo>(student);
            //Ok - 200 -  Success
            return Ok(studentDto);
        }

        [HttpPost]
        [Route("Create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StudentDTo), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTo>> CreateStudent([FromBody] StudentDTo dto)
        {
            if (dto == null)
                return BadRequest("Invalid student data provided.");

            Student student = _mapper.Map<Student>(dto);

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            dto.Id = student.Id;

            return CreatedAtRoute("GetStudentById", new { id = dto.Id }, dto);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTo dto)
        {
            if (dto == null || dto.Id <= 0)
                return BadRequest("Invalid data provided.");

            var existingStudent = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == dto.Id);

            if (existingStudent == null)
                return NotFound($"Student with ID {dto.Id} not found.");

            var newRecord = _mapper.Map<Student>(dto);
            _dbContext.Students.Update(newRecord);

            //existingStudent.StudentName = model.StudentName;
            //existingStudent.Email = model.Email;
            //existingStudent.Address = model.Address;
            //existingStudent.DOB = model.DOB;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTo> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest("Invalid data provided.");

            var existingStudent = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (existingStudent == null)
                return NotFound($"Student with ID {id} not found.");

            var studentDTo = _mapper.Map<StudentDTo>(existingStudent);

            patchDocument.ApplyTo(studentDTo, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDTo);

            _dbContext.Students.Update(existingStudent);

            await _dbContext.SaveChangesAsync();
            //204 - NoContent
            return NoContent();
        }

        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID provided.");

            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return Ok(true);
        }
    }
}
