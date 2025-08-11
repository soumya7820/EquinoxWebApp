using Equinox.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Equinox.Models.DataLayer
{
    public class EquinoxDbContext : DbContext
    {
        public EquinoxDbContext(DbContextOptions<EquinoxDbContext> options)
            : base(options) { }

        public DbSet<ClassCategory> ClassCategory { get; set; } = null!;
        public DbSet<Club> Club { get; set; } = null!;
        public DbSet<EquinoxClass> EquinoxClass { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Booking> Booking { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "312-111-1111" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "312-222-2222" },
                new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "312-333-3333" }
            );
            modelBuilder.Entity<ClassCategory>().HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Boxing", Image = "Boxing" },
                new ClassCategory { ClassCategoryId = 2, Name = "Yoga", Image = "Yoga" },
                new ClassCategory { ClassCategoryId = 3, Name = "HIIT", Image = "HIIT" },
                new ClassCategory { ClassCategoryId = 4, Name = "Strength", Image = "Strength" },
                new ClassCategory { ClassCategoryId = 5, Name = "Dancing", Image = "Dancing" }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "John Smith",
                    PhoneNumber = "555-000-0001",
                    Email = "john.smith@equinox.com",
                    DOB = new DateTime(2000, 8, 7),
                    IsCoach = true
                },
                new User
                {
                    UserId = 2,
                    Name = "Emily Johnson",
                    PhoneNumber = "555-000-0002",
                    Email = "emily.johnson@equinox.com",
                    DOB = new DateTime(2001, 8, 7),
                    IsCoach = true
                }
            );

            modelBuilder.Entity<EquinoxClass>().HasData(
                new EquinoxClass
                {
                    EquinoxClassId = 1,
                    Name = "Boxing 101",
                    ClassPicture = "boxing.png",
                    ClassDay = "Monday",
                    Time = "8 AM – 9 AM",
                    ClassCategoryId = 1,
                    ClubId = 1,
                    UserId = 1
                },
                new EquinoxClass
                {
                    EquinoxClassId = 2,
                    Name = "Hatha Yoga",
                    ClassPicture = "yoga.png",
                    ClassDay = "Wednesday",
                    Time = "6 PM – 7 PM",
                    ClassCategoryId = 2,
                    ClubId = 2,
                    UserId = 2
                },
                new EquinoxClass
                {
                    EquinoxClassId = 3,
                    Name = "HIIT Junior",
                    ClassPicture = "hiit.png",
                    ClassDay = "Friday",
                    Time = "5 PM – 6 PM",
                    ClassCategoryId = 3,
                    ClubId = 3,
                    UserId = 1
                }
            );
        }
    }
}
