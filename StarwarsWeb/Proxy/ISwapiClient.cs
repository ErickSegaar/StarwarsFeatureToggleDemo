using Refit;
using StarwarsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarwarsWeb.Proxy
{
    public interface ISwapiClient
    {
        [Get("/api/people")]
        Task<StarwarsPeopleViewModel> GetPeople();

       
    }
}
