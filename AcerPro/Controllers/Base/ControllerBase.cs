using AcerPro.Dto;
using AcerPro.Dto.ViewModels;
using System.Web.Mvc;

namespace AcerPro.Controllers.Base
{
    public class ControllerBase : Controller
    {
        private UserViewModel currentSession;

        public UserViewModel CurrentSession { get { return currentSession; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            currentSession = (UserViewModel)Session[Constants.SessionKey];
            if (currentSession == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("LoginView", "Account"));
            }

            ViewBag.CurrentSession = currentSession;
            base.OnActionExecuting(filterContext);
        }
    }
}