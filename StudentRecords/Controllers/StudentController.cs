using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRecords.DTOs;
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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            string sql = "SELECT s.Id, s.Code, s.Name, s.Email, s.Mobile, s.Address1, s.Address2, s.IsActive, s.Gender, s.MaritalStatus, s.CityId, s.CreatedBy, s.CreatedOn, s.ModifiedBy, s.ModifiedOn, s.StateId, st.Name AS StateName, c.Name AS CityName " +
                         "FROM Students s " +
                         "LEFT OUTER JOIN States st ON s.StateId = st.SId " +
                         "LEFT OUTER JOIN Cities c ON s.CityId = c.CId";

            var result = await _studentContext.Students.FromSqlRaw(sql)
             .Select(s => new StudentDto
             {
                 Id = s.Id,
                 Code = s.Code,
                 Name = s.Name,
                 Email = s.Email,
                 Mobile = s.Mobile,
                 Address1 = s.Address1,
                 Address2 = s.Address2,
                 IsActive = s.IsActive,
                 Gender = s.Gender,
                 MaritalStatus = s.MaritalStatus,
                 CityId = s.CityId,
                 CreatedBy = s.CreatedBy,
                 CreatedOn = s.CreatedOn,
                 ModifiedBy = s.ModifiedBy,
                 ModifiedOn = s.ModifiedOn,
                 StateId = s.StateId,
                 StateName = s.State.Name,
                 CityName = s.City.Name
             })
         .ToListAsync();
            return Ok(result);
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
        public async Task<IActionResult> PostStudent([FromBody] StudentInputDto studentInput)
        {
            if (ModelState.IsValid)
            {
                var newStudent = new Student
                {
                    Code = studentInput.Code,
                    Name = studentInput.Name,
                    Email = studentInput.Email,
                    Mobile = studentInput.Mobile,
                    Address1 = studentInput.Address1,
                    Address2 = studentInput.Address2,
                    StateId = studentInput.StateId,
                    CityId = studentInput.CityId,
                    Gender = studentInput.Gender,
                    MaritalStatus = studentInput.MaritalStatus,
                    IsActive = studentInput.IsActive,
                    CreatedBy = studentInput.CreatedBy,
                    CreatedOn = studentInput.CreatedOn,
                    ModifiedBy = studentInput.ModifiedBy,
                    ModifiedOn = studentInput.ModifiedOn
                };
                string code = $"{DateTime.Now.ToString("yyyyMMdd")}";
                newStudent.Code = code;

                _studentContext.Students.Add(newStudent);
                await _studentContext.SaveChangesAsync();

                code = code+$"{newStudent.Id:D5}";
                newStudent.Code = code;

                await _studentContext.SaveChangesAsync();

                return Ok(newStudent);
            }

            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentInputDto studentInput)
        {
            var existingStudent = await _studentContext.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.Name = studentInput.Name;
            existingStudent.Email = studentInput.Email;
            existingStudent.Mobile = studentInput.Mobile;
            existingStudent.Address1 = studentInput.Address1;
            existingStudent.Address2 = studentInput.Address2;
            existingStudent.StateId = studentInput.StateId;
            existingStudent.CityId = studentInput.CityId;
            existingStudent.Gender = studentInput.Gender;
            existingStudent.MaritalStatus = studentInput.MaritalStatus;
            existingStudent.IsActive = studentInput.IsActive;
            existingStudent.CreatedBy = studentInput.CreatedBy;
            existingStudent.CreatedOn = studentInput.CreatedOn;
            existingStudent.ModifiedBy = studentInput.ModifiedBy;
            existingStudent.ModifiedOn = studentInput.ModifiedOn;

            _studentContext.Entry(existingStudent).State = EntityState.Modified;

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

