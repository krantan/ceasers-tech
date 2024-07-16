using Dapper;
using MediatR;
using api.Business.Data;
using api.Controllers;

namespace api.Business.Queries
{
    public class GetAccount : IRequest<GetAccountResult>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }

    public class GetAccountHandler : IRequestHandler<GetAccount, GetAccountResult>
    {
        public readonly GuestContext _context;
        public GetAccountHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<GetAccountResult> Handle(GetAccount request, CancellationToken cancellationToken)
        {
            ValidateData.PrintJson(request);
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new BadHttpRequestException($"Invalid Account");
            }
            var query = @"SELECT * FROM [Account] WHERE Username=@Username";

            var rs = await _context.Connection.QueryAsync<Account>(query, new
            {
                Username = request.Username
            });

            if (rs.Count() != 1) {
                throw new BadHttpRequestException($"Invalid Account");
            }

            var account = rs.FirstOrDefault();
            if (account == null) 
            {
                throw new BadHttpRequestException($"Invalid Account");
            }
            var password = CryptData.Hash(request.Password, account.Salt);

            if (account.Password!=password) {
                throw new BadHttpRequestException($"Invalid Account");
            }

            return new GetAccountResult()
            {
                Id = account.Id
            };
        }
    }

    public class GetAccountResult : BaseResponse
    {
        public string Id { get; set; } = "Invalid";

    }
}
