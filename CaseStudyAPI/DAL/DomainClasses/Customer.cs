using CaseStudyAPI.DAL.DAO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudyAPI.DAL.DomainClasses
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Firstname { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required]
        public string? Email { get; set; }
        
        [Required]
        public string? Hash { get; set; }
        [Required]
        public string? Salt { get; set; }

        public static implicit operator Customer(CustomerDAO v)
        {
            throw new NotImplementedException();
        }
    }
}