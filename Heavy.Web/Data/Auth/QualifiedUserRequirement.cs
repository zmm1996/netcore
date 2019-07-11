using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data.Auth
{
    public class QualifiedUserRequirement:IAuthorizationRequirement
    {
        //什么都不干，让它的派生类去做处理
    }
}
