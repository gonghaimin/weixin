using Microsoft.EntityFrameworkCore;
using Rns.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rns.Data
{
    public class RnsContext:DbContext
    {
        public RnsContext(DbContextOptions<RnsContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
