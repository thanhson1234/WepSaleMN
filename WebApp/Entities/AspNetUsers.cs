using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


 namespace WebApp.Entities
{
    [Table("AspNetUsers")]
    public class AspNetUsers : IdentityUser<int>
    {
        public string Password { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public int? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public int? UpdatedUid { get; set; }
        public int? Status { get; set; }
        public string? Avatar { get; set; }

		public int? DeletedBy { get; set; }

        
    }
}
