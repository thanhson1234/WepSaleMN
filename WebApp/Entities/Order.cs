using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Order")]

    public class Order : BaseEntity
    {
        public Order() { }
        public string Deliver { get; set; }
        public string Shiper { get; set; }
        public string AddressBuyer { get; set; }
        public DateTime DateProduct { get; set; }


    }
}
