using Microsoft.AspNetCore.Mvc;

namespace Project2EmailNight.ViewComponents.AdminLayout
{
    public class _AdminLayoutScriptsComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
