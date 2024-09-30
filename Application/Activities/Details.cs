using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }
        }


    }
}

/* An example how to solve with Results from using Microsoft.AspNetCore.Http;
 * 
 * From early stage of development, where no Projections were used.
 * Still a good example to see the use of Microsoft.AspNetCore.Http.
 *  
public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
{
    var activity = await _context.Activities.FindAsync(request.Id);
        if(activity != null)
        {
            return Results.Ok(activity);
        } else
        {
            return Results.NotFound<Activity>(activity);
        }
              
}
 */
