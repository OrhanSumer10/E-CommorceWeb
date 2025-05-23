using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.DataAcsess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()

    {
      
        /*
         EfEntityRepositoryBase<TEntity, TContext>: Bu sınıf, iki generik tür parametresi kullanır:
        TEntity ve TContext.
    IEntityRepository<TEntity>: Bu sınıf, IEntityRepository<TEntity> arayüzünü implement eder. 
        Bu, EfEntityRepositoryBase sınıfının, IEntityRepository<TEntity> 
        arayüzünde tanımlanan yöntemleri gerçekleştireceği anlamına gelir.
         */
        public void Add(TEntity entity)
        {
            using (var context = new TContext()) //using Bloku: Bu blok, TContext sınıfından bir örnek (context) oluşturur ve bu örneği kullanarak veri erişim işlemlerini gerçekleştirir.
            {
                var addedEntity = context.Entry(entity); //Bu metod, verilen entity nesnesini bağlam (context) ile ilişkilendirir ve bu varlık hakkında bilgi sağlar.
                addedEntity.State = EntityState.Added;//Entry metodu, entity nesnesinin EF Core'un takip ettiği bir EntityEntry nesnesini döndürür. EntityEntry, varlığın durumunu yönetmek ve onu veritabanına kaydetmek için kullanılan bir nesnedir.
                context.SaveChanges();//çağrıldığında, EF Core, bağlamda yapılan değişiklikleri (bu durumda yeni varlık ekleme) veritabanına uygular.
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deleted = context.Remove(entity);
                deleted.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }


        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext()) // Yeni bir DbContext örneği oluşturuluyor ve bu örnek using bloğu içinde yönetiliyor. Using bloğu, iş tamamlandığında kaynakların serbest bırakılmasını sağlar.
            {
                return context.Set<TEntity>() // DbContext'teki TEntity türünden varlıkları temsil eden DbSet'i alır.
                    .FirstOrDefault(filter); // Verilen filtreye uyan tek bir varlığı alır. Eğer filtreye uyan birden fazla varlık varsa bir istisna fırlatır. Eğer filtreye uyan hiçbir varlık yoksa, null döner.
            }
      
        }


        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext()) // Yeni bir DbContext örneği oluşturuluyor ve bu örnek using bloğu içinde yönetiliyor. Using bloğu, iş tamamlandığında kaynakların serbest bırakılmasını sağlar.
            {
                return filter == null // Eğer filtre (filter) null (boş) ise,
                    ? context.Set<TEntity>().ToList() // tüm varlıkları (TEntity) veri tabanından çekip liste olarak döndür.
                    : context.Set<TEntity>().Where(filter).ToList(); // Aksi halde, filtreyi (filter) uygulayarak belirtilen şartlara uyan varlıkları çekip liste olarak döndür.
            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updated = context.Update(entity);
                updated.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
