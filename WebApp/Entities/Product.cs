using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("Product")]

    public class Product : BaseEntity
    {
        public Product() { }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public long? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Avatar { get; set; }  
        public string? Description { get; set; }
        public int? NhaCungCap { get; set; } //company
        public int? ThuongHieu { get; set; } //br
        public int? LoaiSanPham { get; set; } //cate
        public int? UnitId { get; set; }
		[NotMapped] 
        
        public int? ShowInHome { get; set; }
        

    }
}
