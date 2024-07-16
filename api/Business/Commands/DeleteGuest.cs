using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using api.Business.Data;
using api.Business.Dtos;
using api.Controllers;

namespace api.Business.Commands
{
    public class DeleteGuest : IRequest<DeleteGuestResult>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteGuestPreProcessor : IRequestPreProcessor<DeleteGuest>
    {
        private readonly GuestContext _context;
        public DeleteGuestPreProcessor(GuestContext context)
        {
            _context = context;
        }
        public Task Process(DeleteGuest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class DeleteGuestHandler : IRequestHandler<DeleteGuest, DeleteGuestResult>
    {
        private readonly GuestContext _context;

        public DeleteGuestHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<DeleteGuestResult> Handle(DeleteGuest request, CancellationToken cancellationToken)
        {
            var processed = false;
            var guest = _context.Guests.Find(request.Id);
            if (guest is not null)
            {
                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
                processed = true;
            }

            return new DeleteGuestResult()
            {
                Processed = processed
            };

        }
    }

    public class DeleteGuestResult : BaseResponse
    {
        public bool Processed { get; set; } = false;
    }
}
