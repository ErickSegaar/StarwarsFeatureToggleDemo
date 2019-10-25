using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarwarsWeb.HardProviders;
using StarwarsWeb.Proxy;

namespace StarwarsWeb.Controllers
{
    public class StarwarsController : Controller
    {
        private readonly ISwapiClient proxy;
        private readonly HardStarshipProvider provider = new HardStarshipProvider();

        public StarwarsController(ISwapiClient proxy)
        {
            this.proxy = proxy;
        }
        public async Task<IActionResult> People()
        {
            return View(await proxy.GetPeople().ConfigureAwait(true));
        }

        public IActionResult Starships()
        {
            return View(provider.GetStarships());
        }
    }
}