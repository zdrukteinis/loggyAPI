using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using loggyAPI.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace loggyAPI.Attributes
{
    /// <summary>
    /// Check if user has required role
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class RoleAttribute : ActionFilterAttribute
    {
        private readonly Role[] _roles;

        public RoleAttribute(params Role[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_roles == null)
            {
                context.HttpContext.Response.ContentType = "application/json";

                context.Result = new JsonResult(
                    new
                    {
                        Messages = new[] { new ApplicationException("Permission denied!") },
                        StatusCode = HttpStatusCode.Unauthorized
                    });

                return;
            }

            var session = context.HttpContext.User.Claims
                .Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value)
                .ToList();

            if (_roles.Any(x => session.Contains(x.ToString())))
            {
                return;
            }

            context.HttpContext.Response.ContentType = "application/json";

            context.Result = new JsonResult(
                new
                {
                    Messages = new[] {new ApplicationException("Permission denied!") },
                    StatusCode = HttpStatusCode.Unauthorized
                });
        }
    }
}
