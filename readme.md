# Demo Simple Feature toggle

- Add packages  
`dotnet add package Microsoft.Azure.AppConfiguration.AspNetCore --version 2.0.0-preview-009470001-12`  
`dotnet add package Microsoft.FeatureManagement.AspNetCore --version 1.0.0-preview-009000001-1251`

- Add StartUp.cs

``` c#
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureTypedClients(services);
            services.AddFeatureManagement();
            services.AddControllersWithViews();
        }
```

- Add tag helper StarwarsWeb/Views/_ViewImports.cshtml

``` html
 @addTagHelper *, Microsoft.FeatureManagement.AspNetCore
```

- Add toggle /StarwarsWeb/Views/Shared/_Layout.cshtml

``` html
<feature name="ShowPlanetsOfStarwars">
     <li class="nav-item">
         <a class="nav-link text-dark" asp-area="" asp-controller="Starwars" asp-action="Planets">Planets</a>
     </li>
 </feature>
 ```

- Show the that the feature toggle hides the implementation of the menu item
- Build out the example ISwapiClient.cs

``` c#
[Get("/api/planets")]
Task<StarwarsPlanets> GetPlanets();
```

- Update StarwarsController.cs

``` c#
public class StarwarsController : Controller
{
    private readonly IFeatureManager featureManager;

    public StarwarsController(ISwapiClient proxy, IFeatureManager featureManager)
    {
        this.proxy = proxy;
        this.featureManager = featureManager;
    }

    //optional if you want to lock on the controller
    //[FeatureGate(FeatureToggles.ShowPlanetsOfStarwars)]
    public async Task<IActionResult> Planets()
    {
        return View(await proxy.GetPlanets().ConfigureAwait(true));
    }

```

- Add other files
  -  StarwarsWeb/FeatureToggles.cs
  -  StarwarsWeb/Models/Planet.cs
  -  StarwarsWeb/Models/StarwarsPlanets.cs
  -  StarwarsWeb/Views/Starwars/Planets.cshtml

- Add application configuration  

``` json
},
  "FeatureManagement": {
     "ShowPlanetsOfStarwars": false
}
```
