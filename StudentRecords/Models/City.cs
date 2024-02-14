using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRecords.Models
{
    public class City
    {
        [Key]
        public int CId { get; set; }
        [ForeignKey("State")]
        public int StateId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}