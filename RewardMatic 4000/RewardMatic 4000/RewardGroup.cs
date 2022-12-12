#nullable enable
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RewardMatic_4000
{
    public record PointRange(uint Start, uint End);

    public class RewardGroup
    {
        [JsonProperty("rewards")]
        public List<Reward> GroupRewards { get; }

        [JsonProperty("name")]
        public string Name { get; }

        public PointRange Range { get; }
        
        public RewardGroup(string name, List<Reward> groupRewards)
        {
            Name = name;
            GroupRewards = groupRewards;

            Range = new PointRange(
                GroupRewards.Min(reward => reward.ScoreDifferential),
                GroupRewards.Max(reward => reward.ScoreDifferential)
            );
        }
    }
}