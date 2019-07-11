using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data.Auth
{
    public class EmailHandel : AuthorizationHandler<EmailRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context
            , EmailRequirement requirement)
        {
            //查找当前用户的Clams名为Email的
            var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Email");
            if (claim != null)
            {
                
                if (claim.Value.EndsWith(requirement.Emailtype))
                {
                    //如果以规定的Email结尾则返回成功
                    context.Succeed(requirement);
                }
            }

            //不通过则不做处理默认的返回--如果这里返回了失败，后面的Requirement就不会执行了
          return  Task.CompletedTask;
        }
    }
}
