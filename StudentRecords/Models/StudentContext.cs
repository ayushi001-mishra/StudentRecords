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
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
