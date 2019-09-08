using Microsoft.EntityFrameworkCore;
using Weixin.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Data
{
    public class WeixinDbContext : DbContext
    {
        public WeixinDbContext(DbContextOptions<WeixinDbContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }

    }
}
