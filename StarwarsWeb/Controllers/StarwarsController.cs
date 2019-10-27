using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using StarwarsWeb.HardProviders;
using StarwarsWeb.Proxy;

namespace StarwarsWeb.Controllers
{
    public class StarwarsController : Controller
    {
        private readonly ISwapiClient proxy;
        private readonly HardStarshipProvider starshipProxy = new HardStarshipProvider();

        public StarwarsController(ISwapiClient proxy)
        {
            this.proxy = proxy;
        }
        public async Task<IActionResult> People()
        {
            return View(await proxy.GetPeople().ConfigureAwait(true));
        }

        public async Task<IActionResult> Starships()
        {
            return View( await starshipProxy.GetStarships().ConfigureAwait(true));
        }
    }
}