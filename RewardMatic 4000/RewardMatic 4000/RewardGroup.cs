#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace RewardMatic_4000
{
    public record PointRange(uint Start, uint End);

    public class RewardGroup
    {
        public List<Reward> GroupRewards { get; }

        public PointRange Range { get; }

        public string Name { get; }
        
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