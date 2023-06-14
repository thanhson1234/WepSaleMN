using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.Models
{

    public class CateViewModel : BaseEntity
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
	
	}
}
