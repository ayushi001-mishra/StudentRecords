using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRecords.Models
{
    public class Student
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public int IsActive { get; set; }

    }

}
