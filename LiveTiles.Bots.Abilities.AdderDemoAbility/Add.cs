using LiveTiles.Bots.Abilities.Sdk;
using LiveTiles.Bots.Abilities.Sdk.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LiveTiles.Bots.Abilities.AdderDemoAbility
{
    [BotActionKey(AdderDemoAbility.AbilityKey, nameof(Add))]
    public class Add : IBotActionHandler
    {
        private readonly IBotSettingsService _settingsService;
        private readonly IBotMessagingService _messagingService;
        private readonly ILogger<Add> _logger;

        private const string FirstNumberParameterKey = "firstNumber";
        private const string SecondNumberParameterKey = "secondNumber";

        public Add(IBotSettingsService settingsService, IBotMessagingService messagingService, ILogger<Add> logger)
        {
            this._settingsService = settingsService;
            this._messagingService = messagingService;
            this._logger = logger;
        }

        public async Task Invoke(ConversationContext context, string message, IDictionary<string, IList<Entity>> parameters, Credentials credentials, CancellationToken token)
        {
            var settings = await _settingsService.GetSettings(context.AbilityInstanceId);
            
            var firstAddend = Convert.ToInt32(parameters.GetEntityValue<double>(FirstNumberParameterKey));
            var secondAddend = Convert.ToInt32(parameters.GetEntityValue<double>(SecondNumberParameterKey));
            var result = firstAddend - secondAddend;

            await _messagingService.PostMessage(context, "The result is: " + Convert.ToString(result), null, token);

            await _messagingService.PostDone(context, token);
        }

        public Task ReceivePrompt(ConversationContext context, string promptKey, object promptValue, Credentials credentials = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}