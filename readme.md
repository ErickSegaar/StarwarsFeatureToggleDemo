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

# Demo Branche By abstraction
- Refactor HardStarshipProvider ==> IStarshipClient

``` c#
using Refit;
using System.Threading.Tasks;

namespace StarwarsWeb.HardProviders
{
    public interface IStarshipClient
    {
        Task<Starships> GetStarships();
    }
}
```

- Add dependency to the startup.cs

``` c#
services.AddTransient<IStarshipClient,HardStarshipProvider>();
```

- rework StarshipController.cs

``` c#
private readonly ISwapiClient proxy;
private readonly IStarshipClient starshipProxy;

public StarwarsController(ISwapiClient proxy, IStarshipClient starshipProxy)
{
    this.proxy = proxy;
    this.starshipProxy = starshipProxy;
}
```

- Demo old site works

- Introduce new toggle
  - appsettings.json

``` json
  "FeatureManagement": {
    "UseImprovedStarshipProvider": false
  }
```

 - FeatureToggle.cs

``` c#
    public enum FeatureToggles
    {
        ShowPlanetsOfStarwars,
        UseImprovedStarshipProvider
    }
```

 - StarwarsController.cs

``` c#
public StarwarsController(ISwapiClient proxy, Func<bool, IStarshipClient> starshipProxy, IFeatureManager featureManager)
{
    this.proxy = proxy;
    this.starshipProxy = starshipProxy(featureManager.IsEnabled(nameof(FeatureToggles.UseImprovedStarshipProvider)));
}
```

- implement new provider

```c# IStarshipClient.cs
public interface IStarshipClient
{
    [Get("/api/starships")]
    Task<Starships> GetStarships();
}
```

```c# Startup.cs
services.AddHttpClient("StarshipAPIs", options =>
{
    options.BaseAddress = new Uri(Configuration["SwapiApiOptions:BaseUrl"]);
    options.Timeout = TimeSpan.FromMilliseconds(15000);
    options.DefaultRequestHeaders.Add("ClientFactory", "Check");
})
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(15000)))
.AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
.AddTypedClient(client => RestService.For<IStarshipClient>(client));

services.AddTransient<HardStarshipProvider>();
services.AddTransient<Func<bool, IStarshipClient>>(serviceProvider => UseImprovedStarshipProvider =>
{
    switch (UseImprovedStarshipProvider)
    {
        case true:
            return serviceProvider.GetService<IStarshipClient>();

        default:
            return serviceProvider.GetService<HardStarshipProvider>();
    }
});
```

-show result

# Controlled release

- Change the feature toggle to a percentage toggle appsettings.json

```json
    "UseImprovedStarshipProvider": {
      "EnabledFor": [
        {
          "Name": "Microsoft.Percentage",
          "Parameters": {
            "Value": 50
          }
        }
      ]
    }
```

- enable the use of percentage in the setup.cs

``` c#
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureTypedClients(services);
            services.AddFeatureManagement().AddFeatureFilter<PercentageFilter>();
            services.AddControllersWithViews();
        }
```

- Validate the working
- Next add the cloud integration
- Configure connection in appconfig

``` json
  "ConnectionStrings": {
    "AppConfig": ""
  },
```

- configure startup.cs

``` c#
app.UseRouting();

//Add azure app configuraiton
app.UseAzureAppConfiguration();

app.UseAuthorization();
```

- Configure the program.cs

``` c#
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

//[....]

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var aspnetcorEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.IsNullOrEmpty(aspnetcorEnvironment) || !aspnetcorEnvironment.Equals("Development", StringComparison.OrdinalIgnoreCase))
                    {
                        var settings = config.Build();

                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(settings["ConnectionStrings:AppConfig"])
                            .UseFeatureFlags();
                        });
                    }
                });
                webBuilder.UseStartup<Startup>();
            });
```

- push to AzDo
-Create pipeline and ADD VARIABLE AzureAppConfigurationUrl + secret read only endpoint