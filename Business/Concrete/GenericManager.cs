using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class, IEntity, new()
    {
        private readonly IGenericService<T> _genericService;
        public GenericManager(IGenericService<T> genericService)
        {
            _genericService = genericService;
        }
        string Message;
        public IResult Add(T t)
        {
            _genericService.Add(t);
            return new SuccessResult(Message);
        }

        public IResult Delete(T t)
        {
            _genericService.Delete(t);
            return new SuccessResult(Message);
        }

        public IDataResult<T> GetById(int id)
        {
            var data = _genericService.GetById(id).Data;

            return new SuccessDataResult<T>(data, Message);
        }

        public IDataResult<List<T>> GetList()
        {
            var data = _genericService.GetList().Data;

            return new SuccessDataResult<List<T>>(data, Message);
        }

        public IResult Update(T t)
        {
            _genericService.Update(t);
            return new SuccessResult(Message);
        }
    }
}
