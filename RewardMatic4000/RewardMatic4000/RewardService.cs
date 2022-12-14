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
            var groupName = GetLatestRewardWithGroupReceived(score)?.RewardGroup?.Name;

            if (groupName is null)
            {
                return null;
            }

            return rewardRepository.GetRewardGroup(groupName);
        }

        public Reward? GetLatestRewardReceived(uint score)
        {
            return GetLatestRewardWithGroupReceived(score);
        }

        public RewardGroup? GetRewardGroupInProgress(uint score)
        {
            var groupName = GetRewardWithGroupInProgress(score)?.RewardGroup?.Name;

            if (groupName is null)
            {
                return null;
            }

            return rewardRepository.GetRewardGroup(groupName);
        }

        public Reward? GetRewardInProgress(uint score)
        {
            return GetRewardWithGroupInProgress(score);
        }

        public RewardGroup? GetLatestCompletedRewardGroup(uint score)
        {
            var groups = rewardRepository.GetRewardGroups();

            if (groups is null || groups.Count() == 0)
            {
                return null;
            }
            
            uint cumulativeScoreForGroupCompletion = 0;
            RewardGroup? candidateGroup = null;
            groups.ToList().ForEach(group =>
            {
                cumulativeScoreForGroupCompletion += group.ScoreForGroupCompletion;
                if (cumulativeScoreForGroupCompletion <= score)
                {
                    candidateGroup = group;
                }
            });

            return candidateGroup;
        }

        private Reward? GetRewardWithGroupInProgress(uint score)
        {
            return rewardRepository.GetRewards()
                .Where(rewardWithGroup => rewardWithGroup.ScoreRequired > score)
                .FirstOrDefault();
        }

        private Reward? GetLatestRewardWithGroupReceived(uint score)
        {
            return rewardRepository.GetRewards()
                .Where(rewardWithGroup => rewardWithGroup.ScoreRequired <= score)
                .LastOrDefault();
        }
    }
}

