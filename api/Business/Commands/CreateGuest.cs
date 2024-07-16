using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using api.Business.Data;
using api.Business.Dtos;
using api.Controllers;

namespace api.Business.Commands
{
    public class CreateGuest : IRequest<CreateGuestResult>
    {
        public string Id { get; set; } = string.Empty;
        public required string NameGiven { get; set; }
        public required string NameFamily { get; set; }
        public required string BirthDate { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Zip { get; set; }
        public required string Phone { get; set; }
        public string Salt { get; set; } = string.Empty;
    }

    public class CreateGuestPreProcessor : IRequestPreProcessor<CreateGuest>
    {
        private readonly GuestContext _context;
        public CreateGuestPreProcessor(GuestContext context)
        {
            _context = context;
        }
        public Task Process(CreateGuest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class CreateGuestHandler : IRequestHandler<CreateGuest, CreateGuestResult>
    {
        private readonly GuestContext _context;

        public CreateGuestHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<CreateGuestResult> Handle(CreateGuest request, CancellationToken cancellationToken)
        {
            var guestInfo = new GuestInfo()
            {
                Id = Guid.NewGuid().ToString(),
                NameGiven = request.NameGiven,
                NameFamily = request.NameFamily,
                BirthDate = request.BirthDate,
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Phone = request.Phone,
                Salt = request.Salt
            };

            var guest = GuestContext.WriteGuest(guestInfo);
            
            if (guest == null)
            {
                throw new BadHttpRequestException($"Invalid Create Guest");
            }

            await _context.Guests.AddAsync(guest);

            await _context.SaveChangesAsync();

            return new CreateGuestResult()
            {
                Id = guest.Id
            };

        }
    }

    public class CreateGuestResult : BaseResponse
    {
        public string Id { get; set; } = string.Empty;
    }
}
