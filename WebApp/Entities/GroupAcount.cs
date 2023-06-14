using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    [Table("GroupAcount")]

    public class GroupAcount : BaseEntity
    {
        public GroupAcount() { }
        public string AccountGroupName { get; set; }
        public int UserId { get; set; }
        public int RoleID { get; set; }
        public int ModuleId { get; set; }
    }
}
