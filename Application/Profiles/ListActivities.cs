using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<PagedList<UserActivityDto>>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
            public PagingParams Params { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<UserActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<PagedList<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.ActivityAttendees
                .Where(u => u.AppUser.UserName == request.Username)
                .OrderBy(a => a.Activity.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();
                query = request.Predicate switch
                {
                    "past" => query.Where(a => a.Date <= DateTime.Now),
                    "hosting" => query.Where(a => a.HostUsername ==
                   request.Username),
                    _ => query.Where(a => a.Date >= DateTime.Now)
                };
                var activities = await PagedList<UserActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);
                return Result<PagedList<UserActivityDto>>.Success(activities);
            }
        }
    }
}
