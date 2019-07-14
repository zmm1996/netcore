using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data.Auth
{
    public class CanEditAlbumHandel : AuthorizationHandler<QualifiedUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QualifiedUserRequirement requirement)
        {




            if (context.User.HasClaim(x => x.Type == "Edit album"))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
