using Core.DataAcsess;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Abstract
{
    public interface IGenericDal<T> : IEntityRepository<T> where T : class,IEntity, new()   
    {
    }
}
