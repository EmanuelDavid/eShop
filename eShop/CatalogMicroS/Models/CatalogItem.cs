using System.ComponentModel.DataAnnotations;

namespace CatalogMicroS.Models
{
    public class CatalogItem
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        // Quantity in stock
        public int AvailableStock { get; set; }
    }
}
