using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarwarsWeb.HardProviders;
using StarwarsWeb.Models;
using StarwarsWeb.Proxy;

namespace StarwarsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ISwapiClient proxy;
        private readonly HardStarshipProvider provider = new HardStarshipProvider();

        public HomeController(ILogger<HomeController> logger, ISwapiClient proxy)
        {
            this.logger = logger;
            this.proxy = proxy;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> People()
        {
            return View(await proxy.GetPeople().ConfigureAwait(true));
        }
        public IActionResult Starships()
        {
            return View(provider.GetStarships());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
