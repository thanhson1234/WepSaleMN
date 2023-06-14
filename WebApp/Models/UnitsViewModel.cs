using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Entities;

namespace WebApp.Models
{
    public class UnitsViewModel : BaseEntity
    {
        public UnitsViewModel() { }
        public string? Name { get; set; }
		public int? Quantity { get; set; }
		public int? ProductId { get; set; }

	}
}
