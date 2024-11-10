using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace PBL_LP.Filters
{
    public class AutorizacaoFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (!allowAnonymous && context.HttpContext.Session.GetString("UsuarioLogado") == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}