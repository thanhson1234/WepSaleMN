using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Entities;

namespace WebApp.Models
{
  
    public class ProductViewModel : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long? Price { get; set; }
        public int? Quantity { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int? NhaCungCap { get; set; } //company
        public int? ThuongHieu { get; set; } //br
        public int? LoaiSanPham { get; set; } //cate

        public string file_anh1 { get; set; }
        public string file_anh2 { get; set; }
		public int? UnitId { get; set; }

		[NotMapped]
        public int? ShowInHome { get; set; }



    }
}
