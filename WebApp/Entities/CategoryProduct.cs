using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("CategoryProduct")]

    public class CategoryProduct : BaseEntity
    {
        public string? Name { get; set; }
        public string? Icon { get; set; } 
    }   
}
