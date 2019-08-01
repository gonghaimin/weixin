using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Core.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
}
