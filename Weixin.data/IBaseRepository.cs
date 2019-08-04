using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Core.Data;

namespace Weixin.Data
{
    public interface IBaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        DbContext Context { get; }
    }
}
