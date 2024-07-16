using Dapper;
using MediatR;
using api.Business.Data;
using api.Business.Dtos;
using api.Controllers;

namespace api.Business.Queries
{
    public class GetGuests : IRequest<GetGuestsResult>
    {
        public uint BlockSize { get; set; } = 10;
        public uint BlockNum { get; set; } = 1;
        public string SortType { get; set; } = "";
    }

    public class GetGuestsHandler : IRequestHandler<GetGuests, GetGuestsResult>
    {
        public readonly GuestContext _context;
        public GetGuestsHandler(GuestContext context)
        {
            _context = context;
        }
        public async Task<GetGuestsResult> Handle(GetGuests request, CancellationToken cancellationToken)
        {
            var validBlockSizes = new List<uint> { 10, 20, 30 };

            if (!ValidateData.IsInList(request.BlockSize, validBlockSizes))
            {
                request.BlockSize = validBlockSizes[0];
            }

            if (request.BlockNum == 0)
            {
                request.BlockNum = 1;
            }
            
            var offset = (request.BlockNum - 1) * request.BlockSize;

            var sortString = GuestInfo.GetSortString(request.SortType);

            var query = @"SELECT * FROM [Guest] WHERE IsActive=1 ORDER BY @Sort LIMIT @Limit OFFSET @Offset";

            var rs = await _context.Connection.QueryAsync<Guest>(query, new
            {
                Sort = sortString,
                Limit = request.BlockSize,
                Offset = offset
            });

            return new GetGuestsResult()
            {
                Guests = rs.Select(x => GuestContext.ReadGuest(x)).ToList(),
            };
        }
    }

    public class GetGuestsResult : BaseResponse
    {
        public List<GuestInfo> Guests { get; set; } = new List<GuestInfo>();
    }
}
