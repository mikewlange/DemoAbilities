using LiveTiles.Bots.Abilities.Sdk;

namespace LiveTiles.Bots.Abilities.AdderDemoAbility
{
    public static class SettingKeys
    {
        public const string FirstNumber = "firstNumber";
        public const string SecondNumber = "secondNumber";
    }

    [BotAbilityKey(AbilityKey)]
    public class AdderDemoAbility : BotAbility
    {
        public const string AbilityKey = "b54b77b9-57ce-4a28-d5b1-08d6874098ae";
    }
}