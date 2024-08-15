using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Domain.Models
{
    public class Portal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public ICollection<Package> Packages { get; set; }
    }
}
