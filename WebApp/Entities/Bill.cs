using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Bill")]

    public class Bill:BaseEntity
    {
        public Bill () { }
     
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public long TotalMonney { get; set;}

    }
}
