using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class IsHostRequirements : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirements>
    {
        private readonly DataContext _dbcontext;
        private readonly IHttpContextAccessor _httpContxtAccessor;

        public IsHostRequirementHandler(DataContext dbcontext, IHttpContextAccessor httpContxtAccessor)
        {
            _dbcontext = dbcontext;
            _httpContxtAccessor = httpContxtAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirements requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;

            var activityId = Guid.Parse(_httpContxtAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());
            var attendee = _dbcontext.ActivityAttendees.AsNoTracking().SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId).Result;

            if (attendee == null) return Task.CompletedTask;
            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
