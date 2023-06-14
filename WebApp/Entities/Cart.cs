using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Cart")]

    public class Cart :BaseEntity
    {
        public Cart() { }
        public string NameProduct { get; set; }
        public int Quantity { get; set; }
        public long UnitPrice { get; set; }
        public long TotalMonney { get; set; }
        public string AddressUser { get; set; }

    }
}
