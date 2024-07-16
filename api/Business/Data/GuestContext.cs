using Microsoft.EntityFrameworkCore;
using System.Data;
using api.Business.Dtos;
using api.Business.Commands;

namespace api.Business.Data
{
    public class GuestContext : DbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Guest> Guests { get; set; }

        public GuestContext(DbContextOptions<GuestContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GuestContext).Assembly);

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
/*
            modelBuilder.Entity<Account>()
                .HasData(
                    new Person
                    {
                        Id = 1,
                        Username = "John Doe"
                    },
                    new Person
                    {
                        Id = 2,
                        Name = "Jane Doe"
                    },
                    new Person
                    {
                        Id = 3,
                        Name = "Duty Test"
                    },
                    new Person
                    {
                        Id = 4,
                        Name = "Test Retired"
                    }
                );
//*/
        }

        public static GuestInfo ReadGuest(Guest request)
        {
            return new GuestInfo()
            {
                Id = request.Id,
                NameGiven = CryptData.Decrypt(request.NameGiven, request.Salt),
                NameFamily = CryptData.Decrypt(request.NameFamily, request.Salt),
                BirthDate = request.BirthDate,
                Email = CryptData.Decrypt(request.Email, request.Salt),
                Address = CryptData.Decrypt(request.Address, request.Salt),
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Phone = CryptData.Decrypt(request.Phone, request.Salt),
                Salt = request.Salt
            };
        }

        public static Guest WriteGuest(GuestInfo request)
        {
            request.Salt = CryptData.GenerateSalt();

            return new Guest()
            {
                Id = request.Id,
                NameGiven = CryptData.Encrypt(request.NameGiven, request.Salt),
                NameFamily = CryptData.Encrypt(request.NameFamily, request.Salt),
                BirthDate = request.BirthDate,
                Email = CryptData.Encrypt(request.Email, request.Salt),
                EmailHash = CryptData.HashQuick(request.Email),
                Address = CryptData.Encrypt(request.Address, request.Salt),
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Phone = CryptData.Encrypt(request.Phone, request.Salt),
                Salt = request.Salt
            };
        }
    }
}
