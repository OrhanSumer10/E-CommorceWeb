using Core.Entities;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGenericService< T> where T : class,IEntity
    {
        IDataResult<T> GetById(int id);
        IDataResult<List<T>> GetList();
        IResult Add(T t);
        IResult Update(T t);
        IResult Delete(T t);
    }
}
