using Domain.Repository;
using Infrastructure.Data;
using System.Collections;

namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable repositories;

        public IVehicleRepository VehicleRepository { get; }

        public UnitOfWork(AppDbContext context, IVehicleRepository vehicleRepository)
        {
            _context = context;
            VehicleRepository = vehicleRepository;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)repositories[type];
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesReturnBool()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
