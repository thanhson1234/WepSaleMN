using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Models
{
	public class RolesViewModel : IdentityRole<int>
	{

		public DateTime? CreatedDateUtc { get; set; }
		public int? CreatedUid { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
		public int? UpdatedUid { get; set; }
		public int? Deleted { get; set; }
		public int? Stt { get; set; }
		public int? DeletedBy { get; set; }
	}
}
