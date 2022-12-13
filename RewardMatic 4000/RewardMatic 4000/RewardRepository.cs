#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

// Visible to the Test assembly in order for it to see the internal constructors
// All other assemblies should only see the static factory method "FromJson"
[assembly: InternalsVisibleTo("RewardMaticTests")]
namespace RewardMatic_4000
{
    public class RewardRepository : IRewardRepository
    {
        private readonly ImmutableList<Reward> _availableRewards;
        private readonly ImmutableList<RewardGroup> _rewardGroups;

        public RewardRepository(List<Reward> rewards)
        {
            _rewardGroups = new List<RewardGroup>().ToImmutableList();
            if (rewards is null)
            {
                _availableRewards = new List<Reward>().ToImmutableList();
            }
            else
            {
                _availableRewards = AttachRequiredScore(0, rewards).ToImmutableList();
            }
        }

        internal RewardRepository(List<RewardGroup> rewardGroups)
        {
            uint groupScoreNeededForCompletion = 0;
            _rewardGroups = rewardGroups.ToImmutableList();

            _rewardGroups.ForEach(rewardGroup =>
            {
                rewardGroup.Rewards = AttachRequiredScore(groupScoreNeededForCompletion, rewardGroup.Rewards);

                groupScoreNeededForCompletion += rewardGroup.ScoreForGroupCompletion;
                rewardGroup.ScoreForGroupCompletion = groupScoreNeededForCompletion;
            });

            _availableRewards = _rewardGroups
                .SelectMany(group => AttachGroup(group, group.Rewards))
                .ToImmutableList();
        }

        public static RewardRepository? FromJson(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                var content = reader.ReadToEnd();

                if (content is null)
                {
                    return null;
                }

                var groups = JsonConvert.DeserializeObject<List<RewardGroup>>(content);
                if (groups is null)
                {
                    return null;
                }

                return new RewardRepository(groups);
            }
        }

        public IEnumerable<Reward> GetRewards()
        {
            return _availableRewards;
        }

        public RewardGroup? GetRewardGroup(string groupName)
        {
            return _rewardGroups.FirstOrDefault(group => group.Name == groupName);
        }

        public Reward? GetReward(string rewardName)
        {
            return _availableRewards.FirstOrDefault(reward => reward.Name == rewardName);
        }

        public IEnumerable<RewardGroup> GetRewardGroups()
        {
            return _rewardGroups;
        }

        private static List<Reward> AttachGroup(RewardGroup group, List<Reward> rewards)
        {
            return rewards.Select(reward => reward with { RewardGroup = group }).ToList();
        }

        private static List<Reward> AttachRequiredScore(uint previousGroupCompletionScore, List<Reward> rewards)
        {
            return rewards.Aggregate(new { sum = (uint)0, items = new List<Reward>() },
                (result, current) =>
                {
                    result.items.Add(current with { ScoreRequired = previousGroupCompletionScore + result.sum + current.ScoreDifferential });
                    return new
                    {
                        sum = result.sum + current.ScoreDifferential,
                        items = result.items
                    };
                }).items;
        }
    }
}
