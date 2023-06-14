using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entities
{
    [Table("Units")]
    public class Units : BaseEntity
    {
        public Units() { }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }

	}
}
