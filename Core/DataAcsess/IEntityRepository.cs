using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAcsess
{
    // veritabınına gönderdiğimiz nesne  IEntity türünde olmalı diyoruz ve newlenebilir olacak.
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {

        /*where T : class: T türünün bir sınıf (class) olması gerektiğini belirtir. 
         * Bu, temel türlerin (örneğin, int gibi değer türleri) T olarak kullanılamayacağını belirtir.
IEntity: T türünün IEntity arayüzünü implement etmesi gerektiğini belirtir. 
        Bu, T türünün belirli bir arayüzü uygulaması gerektiği anlamına gelir.
        IEntity, genellikle tüm varlıkların ortak özelliklerini tanımlayan bir arayüzdür.
new(): T türünün bir parametresiz yapıcıya (constructor) sahip olması gerektiğini belirtir.
        Bu, T türünün yeni bir örneğini oluşturabilmek için gereklidir.*/
        T Get(Expression<Func<T, bool>> filter);
        /*
        Get: Bu yöntem, bir filter ifadesine göre bir T türündeki varlığı alır.
        Expression<Func<T, bool>> filter: Bu, LINQ sorguları için kullanılan bir filtre ifadesidir.
        Bu ifade, hangi koşullara göre verinin alınacağını belirtir. Örneğin, p => p.Id == 1 gibi bir ifade olabilir.
        Yöntem, belirtilen filtreye uyan tek bir varlık (T) döndürür.
        */
        IList<T> GetList(Expression<Func<T, bool>> filter = null);
        /*
         GetList: Bu yöntem, bir filter ifadesine göre bir liste (IList<T>) döndürür.
Expression<Func<T, bool>> filter = null: Bu, isteğe bağlı bir filtre ifadesidir. Eğer filter belirtilmemişse (null), tüm varlıklar döndürülür.
Yöntem, belirtilen filtreye uyan varlıkların bir listesini döndürür.
         */
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
