using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Company")]
    public class Company : BaseEntity
    {
        public Company() { }
        public string? Name { get; set; }
        public string? Evaluate { get; set; }
        public string? Avatar { get; set; }
        public int? ProvinceId { get; set; }
        public DateTime? FoundedTime{ get; set; }
 

    }
}
