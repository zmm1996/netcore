using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data.Auth
{
    public class AdministratorsHandel : AuthorizationHandler<QualifiedUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QualifiedUserRequirement requirement)
        {
            if (context.User.IsInRole("administrators"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
