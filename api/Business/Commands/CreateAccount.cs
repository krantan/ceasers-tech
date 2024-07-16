using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using api.Business.Data;
using api.Controllers;

namespace api.Business.Commands
{
    public class CreateAccount : IRequest<CreateAccountResult>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class CreateAccountPreProcessor : IRequestPreProcessor<CreateAccount>
    {
        private readonly GuestContext _context;
        public CreateAccountPreProcessor(GuestContext context)
        {
            _context = context;
        }
        public Task Process(CreateAccount request, CancellationToken cancellationToken)
        {
            var account = _context.Accounts.AsNoTracking().FirstOrDefault(z => z.Username == request.Username);

            if (account is not null) {
                var message = $"Account Exists `{account.Username}` `{account.Id}`.";
                throw new BadHttpRequestException($"Bad Request::{message}");
            }

            return Task.CompletedTask;
        }
    }

    public class CreateAccountHandler : IRequestHandler<CreateAccount, CreateAccountResult>
    {
        private readonly GuestContext _context;

        public CreateAccountHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<CreateAccountResult> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.Id, out _))
            {
                request.Id = Guid.NewGuid().ToString();
            }

            var salt = CryptData.GenerateSalt();

            var account = new Account()
            {
                Id = request.Id,
                Username = request.Username,
                Password = CryptData.Hash(request.Password, salt),
                Salt = salt
            };

            await _context.Accounts.AddAsync(account);

            await _context.SaveChangesAsync();

            return new CreateAccountResult()
            {
                Id = account.Id
            };
        }
    }

    public class CreateAccountResult : BaseResponse
    {
        public string Id { get; set; } = string.Empty;
    }
}
