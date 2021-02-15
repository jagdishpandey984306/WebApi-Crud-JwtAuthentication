using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Shared.Model.Contact
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //For Identity Column With Unique Values
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PermanentAddress { get; set; }
        [Required]
        public string TemporaryAddress { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string GrandFatherName { get; set; }
        [Required]
        public string MobileNo { get; set; }
    }
}
