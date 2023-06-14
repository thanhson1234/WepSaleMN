using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;

namespace WebApp.Models
{
	public class ModelViewModel 
	{
		public int Id { set; get; }
		public string Name { get; set; }
		public string link { get; set; }
		public int? isEF { get; set; } //1 la FE 2 la Ad
		public string? Logo { get; set; }
		public int? Status { get; set; }

	}
}
