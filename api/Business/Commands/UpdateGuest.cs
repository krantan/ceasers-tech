using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using api.Business.Data;
using api.Business.Dtos;
using api.Controllers;

namespace api.Business.Commands
{
    public class UpdateGuest : IRequest<UpdateGuestResult>
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
    }

    public class UpdateGuestPreProcessor : IRequestPreProcessor<UpdateGuest>
    {
        private readonly GuestContext _context;
        public UpdateGuestPreProcessor(GuestContext context)
        {
            _context = context;
        }
        public Task Process(UpdateGuest request, CancellationToken cancellationToken)
        {
            ValidateData.PrintJson(request);
            return Task.CompletedTask;
        }
    }

    public class UpdateGuestHandler : IRequestHandler<UpdateGuest, UpdateGuestResult>
    {
        private readonly GuestContext _context;

        public UpdateGuestHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<UpdateGuestResult> Handle(UpdateGuest request, CancellationToken cancellationToken)
        {
            bool processed = false;

            var guestInfo = new GuestInfo()
            {
                Id = request.Id,
                NameGiven = request.NameGiven,
                NameFamily = request.NameFamily,
                BirthDate = request.BirthDate,
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Phone = request.Phone
            };

            var guest = GuestContext.WriteGuest(guestInfo);
            
            if (guest == null)
            {
                throw new BadHttpRequestException($"Invalid Create Guest");
            }

            _context.Guests.Update(guest);

            await _context.SaveChangesAsync();

            return new UpdateGuestResult()
            {
                Processed = processed
            };

        }
    }

    public class UpdateGuestResult : BaseResponse
    {
        public bool Processed { get; set; } = false;
    }
}
