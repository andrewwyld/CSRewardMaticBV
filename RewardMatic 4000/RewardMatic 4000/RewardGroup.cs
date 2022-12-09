#nullable enable
using System.Collections.Generic;

namespace RewardMatic_4000
{
    public class RewardGroup
    {
        private readonly List<Reward> _groupRewards;

        public string Name { get; }
        
        public RewardGroup(string name, List<Reward> groupRewards)
        {
            Name = name;
            _groupRewards = groupRewards;
        }

        public Reward? GetRewardByIndex(int i)
        {
            if (i < _groupRewards.Count)
            {
                return _groupRewards[i];
            }
            else
            {
                return null;
            }
        }
    }
}