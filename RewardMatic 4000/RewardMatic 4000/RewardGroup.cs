#nullable enable
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RewardMatic_4000
{
    public class RewardGroup
    {
        [JsonProperty("rewards")]
        public List<Reward> Rewards { get; set; }

        [JsonProperty("name")]
        public string Name { get; }

        public uint ScoreForGroupCompletion { get; set; }

        [JsonConstructor]
        public RewardGroup(string name, List<Reward> rewards)
        {
            Name = name;
            Rewards = rewards;

            ScoreForGroupCompletion =
                Rewards.Aggregate((uint)0, (sum, current) => sum + current.ScoreDifferential);
        }
    }
}