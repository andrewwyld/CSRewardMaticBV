#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace RewardMatic_4000
{
	public class RewardService: IRewardService
	{
        private readonly IRewardRepository rewardRepository;

        public RewardService(IRewardRepository rewardRepository)
		{
            this.rewardRepository = rewardRepository;
        }

        public RewardGroup? GetLatestRewardGroupReceived(uint score)
        {
            var groupName = GetLatestRewardWithGroupReceived(score)?.groupName;

            if (groupName is null)
            {
                return null;
            }

            return rewardRepository.GetRewardGroup(groupName);
        }

        public Reward? GetLatestRewardReceived(uint score)
        {
            return GetLatestRewardWithGroupReceived(score)?.reward;
        }

        public RewardGroup? GetRewardGroupInProgress(uint score)
        {
            var groupName = GetRewardWithGroupInProgress(score)?.groupName;

            if (groupName is null)
            {
                return null;
            }

            return rewardRepository.GetRewardGroup(groupName);
        }

        public Reward? GetRewardInProgress(uint score)
        {
            return GetRewardWithGroupInProgress(score)?.reward;
        }

        public RewardGroup? GetLatestCompletedRewardGroup(uint score)
        {
            var groups = rewardRepository.GetRewardGroups();

            if (groups is null || groups.Count() == 0)
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

        private RewardWithGroup? GetRewardWithGroupInProgress(uint score)
        {
            return rewardRepository.GetRewards()
                .Where(rewardWithGroup => rewardWithGroup.reward.ScoreDifferential > score)
                .FirstOrDefault();
        }

        private RewardWithGroup? GetLatestRewardWithGroupReceived(uint score)
        {
            return rewardRepository.GetRewards()
                .Where(rewardWithGroup => rewardWithGroup.reward.ScoreDifferential <= score)
                .LastOrDefault();
        }
    }
}

