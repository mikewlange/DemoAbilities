using LiveTiles.Bots.Abilities.HelloWorldDemoAbility.Strings;
using LiveTiles.Bots.Abilities.Sdk;
using LiveTiles.Bots.Abilities.Sdk.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LiveTiles.Bots.Abilities.HelloWorldDemoAbility
{
    [BotActionKey(HelloWorldDemoAbility.AbilityKey, nameof(Speak))]
    public class Speak : IBotActionHandler
    {
        private readonly IBotSettingsService _settingsService;
        private readonly IBotMessagingService _messagingService;
        private readonly ILogger<Speak> _logger;

        public Speak(IBotSettingsService settingsService, IBotMessagingService messagingService, ILogger<Speak> logger)
        {
            this._settingsService = settingsService;
            this._messagingService = messagingService;
            this._logger = logger;
        }

        public async Task Invoke(ConversationContext context, string message, IDictionary<string, IList<Entity>> parameters, Credentials credentials, CancellationToken token)
        {
            await _messagingService.PostMessage(context, Vocabulary.Response_Message, null, token);
            await _messagingService.PostDone(context, token);
        }

        public Task ReceivePrompt(ConversationContext context, string promptKey, object promptValue, Credentials credentials = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}