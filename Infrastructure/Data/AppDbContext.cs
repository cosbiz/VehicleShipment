using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		// DbSets for your entities
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Transport> Transports { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Ensure you call the base method so Identity-related tables are configured
			base.OnModelCreating(modelBuilder);

			// Vehicle Configuration (One-to-One with User)
			modelBuilder.Entity<Vehicle>()
				.HasKey(v => v.Id);  // Primary key for Vehicle

            modelBuilder.Entity<Vehicle>()
				.HasOne(v => v.User)  // Each vehicle has an optional user
				.WithOne(u => u.Vehicle)  // Each user has an optional vehicle
				.HasForeignKey<Vehicle>(v => v.UserId)  // Vehicle's foreign key is UserId
				.IsRequired(false);  // Make the foreign key optional

            // Transport Configuration (Many-to-One with User)
            modelBuilder.Entity<Transport>()
				.HasKey(t => t.Id);  // Primary key for Transport

			modelBuilder.Entity<Transport>()
				.HasOne(t => t.User)  // Each transport has one user
				.WithMany(u => u.Transports)  // Each user can have many transports
				.HasForeignKey(t => t.UserId)  // Transport's foreign key is UserId
				.IsRequired(true);  // Enforce a required relationship
		}
	}
}
