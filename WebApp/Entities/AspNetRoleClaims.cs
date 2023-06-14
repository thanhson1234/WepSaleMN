using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entities
{
    [Table("AspNetRoleClaims")]
    public class AspNetRoleClaims : IdentityRoleClaim<int>
    {
        public int? ModuleValue { get; set; }
    }

    [Table("AspNetUserClaims")]
    public class AspNetUserClaims : IdentityUserLogin<int>
    {
        [Key]
        public int Id { get; set; }
        public int? ClaimValue { get; set; }
    }
}
