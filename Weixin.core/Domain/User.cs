using Weixin.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Weixin.Core.Domain
{
    [Table("user")]
    public class User:BaseEntity
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string SaltKey { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        [NotMapped]
        public string[] Permissions { get; set; }
    }
}
