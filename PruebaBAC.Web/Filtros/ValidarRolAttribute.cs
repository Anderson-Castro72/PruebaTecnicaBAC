using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PruebaBAC.Web.Filtros
{
    public class ValidarRolAttribute : ActionFilterAttribute
    {
        private readonly string _rolRequerido;

        public ValidarRolAttribute(string rolRequerido)
        {
            _rolRequerido = rolRequerido;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rolUsuario = context.HttpContext.Session.GetString("UsuarioRol");

            if (string.IsNullOrEmpty(rolUsuario))
            {
                context.Result = new RedirectToActionResult("Login", "Acceso", null);
                return;
            }

            if (rolUsuario != _rolRequerido)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}