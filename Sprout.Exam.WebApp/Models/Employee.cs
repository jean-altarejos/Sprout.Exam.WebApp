using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.WebApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string Tin { get; set; }
        public int TypeId { get; set; }
    }
}
