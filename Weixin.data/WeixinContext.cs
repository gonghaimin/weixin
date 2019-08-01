using Microsoft.EntityFrameworkCore;
using Weixin.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Data
{
    public class WeixinContext : DbContext
    {
        public WeixinContext(DbContextOptions<WeixinContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }

    }
}
