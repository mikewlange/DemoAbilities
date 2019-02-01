using LiveTiles.Bots.Abilities.AzureFunctions;
using LiveTiles.Bots.Abilities.AdderDemoAbility;
using LiveTiles.Bots.Abilities.Sdk.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

[assembly: WebJobsStartup(typeof(Startup))]

namespace LiveTiles.Bots.Abilities.AdderDemoAbility
{
    public class Startup : IStartup
    {
        public static IConfiguration Configuration { get; private set; }

        public static IServiceProvider Services { get; private set; }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.UseStartup(this);
        }

        public IConfiguration Configure(IConfigurationBuilder app)
        {
            Configuration = app
                .SetBasePath(Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") + "\\Configuration")
                .AddJsonFile("settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var abilityConfig = Configuration.GetValue<AbilityConfiguration>(nameof(AbilityConfiguration));

            services.AddBotAbilities(Configuration.GetSection(nameof(AbilityConfiguration)));

            Services = services.BuildServiceProvider();
        }
    }
}