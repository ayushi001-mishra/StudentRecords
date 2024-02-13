using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRecords.Models;

namespace StudentRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _studentContext;
        public StudentController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetEmployees()
        {
            if(_studentContext.Students == null)
            {
                return NotFound();
            }
            return await _studentContext.Students.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            if (_studentContext.Students == null)
            {
                return NotFound();
            }
            var student = await _studentContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            string studentCode = $"{DateTime.Now.ToString("yyyyMMdd")}{student.Id}";
            student.Code = studentCode;

            _studentContext.Students.Add(student);
            await _studentContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _studentContext.Entry(student).State = EntityState.Modified;

            try
            {
                await _studentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                 throw;
            }

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent(int id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var student = await _studentContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(student, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _studentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(student);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_studentContext.Students == null)
            {
                return NotFound();
            }

            var student = await _studentContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _studentContext.Students.Remove(student);   
            await _studentContext.SaveChangesAsync();

            return Ok();
        }
    }
}

