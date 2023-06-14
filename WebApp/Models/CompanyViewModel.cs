using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;

namespace WebApp.Models
{
    public class CompanyViewModel : BaseEntity
    {
        public string? Name { get; set; }
        public string? Evaluate { get; set; }
        public string? Avatar { get; set; }
        public int? ProvinceId { get; set; }
        public DateTime? FoundedTime { get; set; }



    }
}
