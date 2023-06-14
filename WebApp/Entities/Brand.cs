using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Brand")]
    public class Brand : BaseEntity
    {
        public Brand() { }

        public string? Name { get; set; }

        public string? Evaluate { get; set; }    
        public int? ProvinceId { get; set; }  
        public DateTime? FoundedTime {  get; set; }
    }
}
