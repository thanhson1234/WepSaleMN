using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("ImgProduct")]

    public class ImgProduct : BaseEntity
    {
        public ImgProduct() { }

        public string IMG { get; set; }
        public int IdProduct { get; set; }
    }
}
