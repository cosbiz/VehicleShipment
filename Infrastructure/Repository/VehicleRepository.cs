using Domain.DTO.Request;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public List<Vehicle> GetVehicles(GetVehicleRequest request)
        {
            IQueryable<Vehicle> query = _dbContext.Set<Vehicle>();

            // If no request is provided, return all vehicles
            if (request == null) return [.. query];

            // Filter by DriverName if provided
            if (!string.IsNullOrEmpty(request.DriverName))
            {
                query = query.Where(x => EF.Functions.Like(x.User.FirstName, $"%{request.DriverLicence}%"));
            }

            // If more filtering criteria are required (e.g., VehicleBrand, VehicleType, etc.),
            // you can add them here in the same way as the DriverName filtering.

            // Return the filtered or full list of vehicles
            return query.ToList();
        }
    }
}
