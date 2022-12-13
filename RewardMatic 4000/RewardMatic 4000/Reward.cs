#nullable enable

using Newtonsoft.Json;

namespace RewardMatic_4000
{
    public record Reward
    (
        [property: JsonProperty("scoredifferential")] uint ScoreDifferential,
        [property: JsonProperty("name")] string Name,
        [property: JsonIgnore] uint ScoreRequired = 0,
        [property: JsonIgnore] RewardGroup? RewardGroup = null);
}