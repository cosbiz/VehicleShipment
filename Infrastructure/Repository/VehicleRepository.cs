using Domain.DTO.Request;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

        public List<Vehicle> GetVehicles(GetVehicleRequest request)
        {
            IQueryable<Vehicle> query = _dbContext.Set<Vehicle>()
                .Include(x => x.Id);

            if (request == null) return query.ToList();

            if(!string.IsNullOrEmpty(request.DriverName))
            {
                query = query.Where(x => (EF.Functions.Like(x.VehicleNumber, $"%{request.DriverLicence}")));
            }

            return query.ToList();
        }
    }
}
