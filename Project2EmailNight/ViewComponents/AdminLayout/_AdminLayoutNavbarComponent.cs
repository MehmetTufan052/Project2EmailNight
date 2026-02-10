using Microsoft.AspNetCore.Mvc;

namespace Project2EmailNight.ViewComponents.AdminLayout
{
    public class _AdminLayoutNavbarComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
