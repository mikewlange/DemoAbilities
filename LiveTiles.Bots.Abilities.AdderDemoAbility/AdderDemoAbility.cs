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
        public const string AbilityKey = "[ABILITY KEY]";
    }
}