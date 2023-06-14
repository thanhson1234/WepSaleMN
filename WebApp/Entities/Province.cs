using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entities
{
    [Table("Province")]
    public class Province : BaseEntity
    {
        public Province() { }
        public string Name { get; set; }
    }
}
