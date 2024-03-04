using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentRecords.DTOs;
using StudentRecords.Models;
using System.Data;

namespace StudentRecords.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _studentContext;
        public StudentController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        [Route("api/Student")]
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
        

        [Route("api/Student/Paginated")]
        [HttpPost]
        public async Task<ActionResult<PaginatedResult<StudentDto>>> GetStudentsPost([FromBody] PaginationParameters paginationParameters)
        {
            int pageNumber = paginationParameters.PageNumber;
            int pageSize = paginationParameters.PageSize;

            int totalActiveStudents = await _studentContext.Students.CountAsync(s => s.IsActive == 1);
            int totalPage = (int)Math.Ceiling((double)totalActiveStudents / pageSize);

            string sql = "SELECT s.Id, s.Code, s.Name, s.Email, s.Mobile, s.Address1, s.Address2, s.IsActive, s.Gender, s.MaritalStatus, s.CityId, s.CreatedBy, s.CreatedOn, s.ModifiedBy, s.ModifiedOn, s.StateId, st.Name AS StateName, c.Name AS CityName " +
                        "FROM Students s " +
                        "LEFT OUTER JOIN States st ON s.StateId = st.SId " +
                        "LEFT OUTER JOIN Cities c ON s.CityId = c.CId";

            var result = await _studentContext.Students.FromSqlRaw(sql)
                .Where(s => s.IsActive == 1)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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

            /*return Ok(result);*/
            return Ok(new PaginatedResult<StudentDto>(result, totalPage));
        }

        [Route("api/Student/PaginatedSort")]
        [HttpPost]
        public async Task<ActionResult<PaginatedResult<StudentDto>>> GetStudentsPaginatedAndSorted([FromBody] PaginationSortParameters parameters)
        {
            int pageNumber = parameters.PageNumber;
            int pageSize = parameters.PageSize;
            string sortAttribute = parameters.SortAttribute.ToLower();
            bool isAscending = parameters.SortOrder == "asc";
            string searchName = parameters.SearchName;

            int totalActiveStudents = await _studentContext.Students.CountAsync(s => s.IsActive == 1);
            int totalPage = (int)Math.Ceiling((double)totalActiveStudents / pageSize);

            string sql = "";
            IQueryable<StudentDto> query;

            if (!string.IsNullOrEmpty(searchName))
            {
                sql = $"SELECT s.Id, s.Code, s.Name, s.Email, s.Mobile, s.Address1, s.Address2, s.IsActive, s.Gender, s.MaritalStatus, s.CityId, s.CreatedBy, s.CreatedOn, s.ModifiedBy, s.ModifiedOn, s.StateId, st.Name AS StateName, c.Name AS CityName " +
                      $"FROM Students s " +
                      $"LEFT OUTER JOIN States st ON s.StateId = st.SId " +
                      $"LEFT OUTER JOIN Cities c ON s.CityId = c.CId " +
                      $"WHERE s.Name = @searchName";

                SqlParameter parameter = new SqlParameter("@searchName", SqlDbType.NVarChar)
                {
                    Value = searchName
                };

                 query = _studentContext.Students.FromSqlRaw(sql, parameter)
                                                            .Where(s => s.IsActive == 1)
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
                                                            });
            }
            else
            {
                sql = "SELECT s.Id, s.Code, s.Name, s.Email, s.Mobile, s.Address1, s.Address2, s.IsActive, s.Gender, s.MaritalStatus, s.CityId, s.CreatedBy, s.CreatedOn, s.ModifiedBy, s.ModifiedOn, s.StateId, st.Name AS StateName, c.Name AS CityName " +
                           "FROM Students s " +
                           "LEFT OUTER JOIN States st ON s.StateId = st.SId " +
                           "LEFT OUTER JOIN Cities c ON s.CityId = c.CId";

                query = _studentContext.Students.FromSqlRaw(sql)
                                                            .Where(s => s.IsActive == 1)
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
                                                            });
            }

            switch (sortAttribute)
            {
                case "name":
                    query = isAscending ? query.OrderBy(s => s.Name) : query.OrderByDescending(s => s.Name);
                    break;
                case "code":
                    query = isAscending ? query.OrderBy(s => s.Code) : query.OrderByDescending(s => s.Code);
                    break;
                case "email":
                    query = isAscending ? query.OrderBy(s => s.Email) : query.OrderByDescending(s => s.Email);
                    break;
                case "mobile":
                    query = isAscending ? query.OrderBy(s => s.Mobile) : query.OrderByDescending(s => s.Mobile);
                    break;
                case "address1":
                    query = isAscending ? query.OrderBy(s => s.Address1) : query.OrderByDescending(s => s.Address1);
                    break;
                case "address2":
                    query = isAscending ? query.OrderBy(s => s.Address2) : query.OrderByDescending(s => s.Address2);
                    break;
                case "gender":
                    query = isAscending ? query.OrderBy(s => s.Gender) : query.OrderByDescending(s => s.Gender);
                    break;
                case "maritalstatus":
                    query = isAscending ? query.OrderBy(s => s.MaritalStatus) : query.OrderByDescending(s => s.MaritalStatus);
                    break;
                case "statename":
                    query = isAscending ? query.OrderBy(s => s.StateName) : query.OrderByDescending(s => s.StateName);
                    break;
                case "cityname":
                    query = isAscending ? query.OrderBy(s => s.CityName) : query.OrderByDescending(s => s.CityName);
                    break;
                default:
                    query = query.OrderBy(s => s.Id); 
                    break;
            }

            var result = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return Ok(new PaginatedResult<StudentDto>(result, totalPage));
        }


        [Route("api/Student/{id}")]
        [HttpGet]
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

        [Route("api/Student")]
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

        [Route("api/Student/{id}")]
        [HttpPut]
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

        [Route("api/Student/{id}")]
        [HttpPatch]
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

        [Route("api/Student/{id}")]
        [HttpDelete]
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

        [Route("api/Student/CheckEmail/{email}")]
        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            var exists = _studentContext.Students.Any(u => u.Email == email);
            return Ok(new { Exists = exists });
        }

        [Route("api/Student/CheckMobile/{mobile}")]
        [HttpGet]
        public IActionResult CheckMobileExists(string mobile)
        {
            var exists = _studentContext.Students.Any(u => u.Mobile == mobile);
            return Ok(new { Exists = exists });
        }
    }
}

