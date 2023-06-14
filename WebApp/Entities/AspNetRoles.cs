using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Entities
{

    [Table("AspNetRoles")]
    public class AspNetRoles : IdentityRole<int>
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
