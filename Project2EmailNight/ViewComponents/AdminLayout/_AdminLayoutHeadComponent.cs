using Microsoft.AspNetCore.Mvc;

namespace Project2EmailNight.ViewComponents.AdminLayout
{
    public class _AdminLayoutHeadComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
