using Core.DataAcsess.EntityFramework;
using Core.Entities;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Concrete.EntityFramework
{
    public class EfGenericDal<T> : EfEntityRepositoryBase<T, MySQLContext>, IGenericDal<T>
      where T : class, IEntity, new()
    {
    }
}
