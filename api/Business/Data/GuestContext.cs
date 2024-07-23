using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;
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
            modelBuilder.Entity<Account>()
                .HasData(
                    new Account
                    {
                        Id = "1c6eca56-1cb9-4e7f-8d56-e9192bc8b9f0",
                        Username = "usera@guestapi.com",
                        Password = "eamUQMMyKM/kicXPv3H5/Lvh/ZopMdSuJeDCB7uvW/JtdeGcKIQWBUKgkPpZApOedGa2HPJe7n7oPC9So/wD3Q==",
                        Salt = "yK8jjLPtHlyWOZWbKo8hdw=="
                    },
                    new Account
                    {
                        Id = "4849630d-685c-42cb-8bea-a71ee19f71ba",
                        Username = "userb@guestapi.com",
                        Password = "/q2GyDODzWB4gIcgZf/CXC8/GHk09bEjwLaumqBNrPl/YiRQLX3W92Njz63QqQu4XBlhdcZv5qDkztrlJXB7Iw==",
                        Salt = "u6V9loynuzGEsRQ/97dJew=="
                    }
                );
        }

        public static GuestInfo ReadGuest(Guest request)
        {
            request.Phone = CryptData.Decrypt(request.Phone, request.Salt);

            if (request.Phone.Length==10)
            {
                request.Phone = Regex.Replace(request.Phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }

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
                Phone = request.Phone,
                Salt = request.Salt
            };
        }

        public static Guest WriteGuest(GuestInfo request)
        {
            request.Salt = CryptData.GenerateSalt();

            var formats = new[] { "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            DateTime dt;
            if (DateTime.TryParseExact(request.BirthDate, formats, null, DateTimeStyles.None, out dt))
            {
                request.BirthDate = dt.ToString("d");
            }

            request.Zip = Regex.Replace(request.Phone, @"[^0-9]", "");
            request.Phone = Regex.Replace(request.Phone, @"[^0-9\+]", "");

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
