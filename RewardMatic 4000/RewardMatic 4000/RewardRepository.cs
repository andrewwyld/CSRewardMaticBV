#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RewardMatic_4000
{
    public record RewardWithGroup(Reward reward, string? groupName = "");

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

        public RewardGroup? GetLatestRewardGroupReceived(uint score)
        {
            var groupName = GetLatestRewardWithGroupReceived(score)?.groupName;

            return getRewardGroupByName(groupName);
        }

        public Reward? GetLatestRewardReceived(uint score)
        {
            return GetLatestRewardWithGroupReceived(score)?.reward;
        }

        public RewardGroup? GetRewardGroupInProgress(uint score)
        {
            var groupName = GetRewardWithGroupInProgress(score)?.groupName;

            return getRewardGroupByName(groupName);
        }

        public Reward? GetRewardInProgress(uint score)
        {
            return GetRewardWithGroupInProgress(score)?.reward;
        }

        public RewardGroup? GetLatestCompletedRewardGroup(uint score)
        {
            var groups = _rewardGroups.Values.ToList();

            if (groups is null)
            {
                return null;
            }

            var completedGroups = groups.Where(group => group.Range.End <= score);

            if (completedGroups.Count() == 0)
            {
                return null;
            }

            return completedGroups.Aggregate((best, current) =>
            {
                return best.Range.End > current.Range.End ? best : current;
            });
        }

        public IEnumerable<Reward> GetRewards()
        {
            return _availableRewards.Select(rewardWithGroup => rewardWithGroup.reward);
        }

        private RewardGroup? getRewardGroupByName(string? groupName)
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

        private RewardWithGroup? GetRewardWithGroupInProgress(uint score)
        {
            return _availableRewards
                .Where(rewardWithGroup => rewardWithGroup.reward.ScoreDifferential > score)
                .FirstOrDefault();
        }

        private RewardWithGroup? GetLatestRewardWithGroupReceived(uint score)
        {
            return _availableRewards
                .Where(rewardWithGroup => rewardWithGroup.reward.ScoreDifferential <= score)
                .LastOrDefault();
        }
    }
}
