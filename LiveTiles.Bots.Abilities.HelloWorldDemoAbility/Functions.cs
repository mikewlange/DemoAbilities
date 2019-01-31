using LiveTiles.Bots.Abilities.Sdk;
using LiveTiles.Bots.Abilities.Sdk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LiveTiles.Bots.Abilities.HelloWorldDemoAbility
{
    public static class Functions
    {
        [FunctionName(nameof(ExecuteAction))]
        public static async Task<IActionResult> ExecuteAction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{abilityKey}/{actionKey}")] ActionCommand req,
            string abilityKey,
            string actionKey,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(typeof(Startup).FullName);

            using (var scope = Startup.Services.CreateScope())
            {
                await scope
                    .ServiceProvider
                    .GetService<IAbilityDispatcher>()
                    .ExecuteAction(req, abilityKey, actionKey, cancellationToken);
            }

            return new OkResult();
        }

        [FunctionName(nameof(ReceivePromptResponse))]
        public static async Task<IActionResult> ReceivePromptResponse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{abilityKey}/{actionKey}/{promptKey}")] PromptResponse req,
            string abilityKey,
            string actionKey,
            string promptKey,
            CancellationToken cancellationToken)
        {
            using (var scope = Startup.Services.CreateScope())
            {
                await scope
                    .ServiceProvider
                    .GetService<IAbilityDispatcher>()
                    .PromptResponse(req, abilityKey, actionKey, promptKey, cancellationToken);
            }

            return new OkResult();
        }
    }
}