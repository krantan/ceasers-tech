using Dapper;
using MediatR;
using api.Business.Data;
using api.Business.Dtos;
using api.Controllers;


namespace api.Business.Queries
{
    public class GetGuestById : IRequest<GetGuestByIdResult>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetGuestByIdHandler : IRequestHandler<GetGuestById, GetGuestByIdResult>
    {
        public readonly GuestContext _context;
        public GetGuestByIdHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<GetGuestByIdResult> Handle(GetGuestById request, CancellationToken cancellationToken)
        {
            if (!ValidateData.IsUuid(request.Id))
            {
                throw new BadHttpRequestException($"Invalid GuestId");
            }

            var query = @"SELECT * FROM [Guest] WHERE IsActive=1 AND Id=@Id";

            var rs = await _context.Connection.QueryAsync<Guest>(query, new
            {
                Id = request.Id
            });

            if (rs.Count() != 1) {
                throw new BadHttpRequestException($"Invalid Guest");
            }

            var row = rs.FirstOrDefault();
            if (row == null) 
            {
                throw new BadHttpRequestException($"Invalid Guest");
            }

            var guest = GuestContext.ReadGuest(row);
            return new GetGuestByIdResult()
            {
                Guest = guest
            };
        }
    }

    public class GetGuestByIdResult : BaseResponse
    {
        public GuestInfo Guest { get; set; } = new GuestInfo();
    }
}
