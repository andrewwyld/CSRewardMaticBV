#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace RewardMatic_4000
{
    public class RewardRepository : IRewardRepository
    {
        private readonly ImmutableList<RewardWithGroup> _availableRewards;
        private readonly ImmutableDictionary<string, RewardGroup> _rewardGroups;


        public RewardRepository(List<Reward> rewards)
        {
            _rewardGroups = new Dictionary<string, RewardGroup>().ToImmutableDictionary();
            if (rewards is null)
            {
                _availableRewards = new List<RewardWithGroup>().ToImmutableList();
            }
            else
            {
                _availableRewards = rewards.Select(reward => new RewardWithGroup(reward)).ToImmutableList();
                _availableRewards = _availableRewards
                    .Sort((reward1, reward2) =>
                        reward1.reward.ScoreDifferential.CompareTo(reward2.reward.ScoreDifferential));
            }
        }

        public RewardRepository(List<RewardGroup> rewardGroups)
        {
            _rewardGroups = rewardGroups.ToImmutableDictionary(
                keySelector: (group) => group.Name,
                elementSelector: (group) => group);

            _availableRewards = rewardGroups
                .SelectMany(group => group.GroupRewards
                    .Select(reward => new RewardWithGroup(reward, group.Name)))
                .ToImmutableList();

            _availableRewards = _availableRewards
                .Sort((reward1, reward2) =>
                    reward1.reward.ScoreDifferential.CompareTo(reward2.reward.ScoreDifferential));
        }

        public static RewardRepository FromJson(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                var content = reader.ReadToEnd();

                if (content is null)
                {
                    return new RewardRepository(new List<RewardGroup>());
                }

                var groups = JsonConvert.DeserializeObject<List<RewardGroup>>(content);
                return new RewardRepository(groups);
            }
        }

        public IEnumerable<RewardWithGroup> GetRewards()
        {
            return _availableRewards;
        }

        public RewardGroup? GetRewardGroup(string groupName)
        {
            if (groupName is null)
            {
                return null;
            }

            if (!_rewardGroups.ContainsKey(groupName))
            {
                return null;
            }
            return _rewardGroups[groupName];
        }

        public Reward? GetReward(string name)
        {
            return _availableRewards
                .FirstOrDefault(rewardWithGroup => rewardWithGroup.reward.Name == name)?
                .reward;
        }

        public IEnumerable<RewardGroup> GetRewardGroups()
        {
            return _rewardGroups.Values;
        }
    }
}
