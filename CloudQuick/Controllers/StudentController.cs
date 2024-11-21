using AutoMapper;
using CloudQuick.Data;
using CloudQuick.Data.Repository;
using CloudQuick.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly ICloudRepository<Student> _studentRepository;

        public StudentController(ILogger<StudentController> logger,
            IMapper mapper, ICloudRepository<Student> studentRepository)
        {
            _logger = logger;
            _mapper =  mapper;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTo>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudents method started");
             var students = await _studentRepository.GetAllAsync();

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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id);
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

            var student = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
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

           var studentAfterCreation =  await _studentRepository.CreateAsync(student);

            dto.Id = studentAfterCreation.Id;

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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == dto.Id, true);

            if (existingStudent == null)
                return NotFound($"Student with ID {dto.Id} not found.");

            var newRecord = _mapper.Map<Student>(dto);
            await _studentRepository.UpdateAsync(newRecord);

            //existingStudent.StudentName = model.StudentName;
            //existingStudent.Email = model.Email;
            //existingStudent.Address = model.Address;
            //existingStudent.DOB = model.DOB;

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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == id, true);
            if (existingStudent == null)
                return NotFound($"Student with ID {id} not found.");

            var studentDTo = _mapper.Map<StudentDTo>(existingStudent);

            patchDocument.ApplyTo(studentDTo, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDTo);



            await _studentRepository.UpdateAsync(existingStudent);

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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id);



            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            await _studentRepository.DeleteAsync(student);

            return Ok(true);
        }
    }
}
