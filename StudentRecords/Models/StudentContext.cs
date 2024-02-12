using Microsoft.EntityFrameworkCore;
using StudentRecords.Models;

namespace StudentRecords.Models
{
    public class StudentContext : DbContext 
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) //ctor
        {
            
        }

        public DbSet<Student> Students { get; set; } = null!;
    }
}
