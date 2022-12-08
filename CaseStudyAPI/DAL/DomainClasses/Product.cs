using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudyAPI.DAL.DomainClasses
{
    public class Product
    {
        [Key]
        [StringLength(15)]
        public string? Id { get; set; }

        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; } // generates FK
        [Required]
        public int BrandId { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[]? Timer { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? GraphicName { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }

        [Required]
        public int QtyOnHand { get; set; }

        [Required]
        public int QtyOnBackorder { get; set; }


        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
