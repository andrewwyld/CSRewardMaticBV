#nullable enable

using System;

namespace RewardMatic_4000
{
	public interface IRewardService
	{
        Reward? GetRewardInProgress(uint score);

        Reward? GetLatestRewardReceived(uint score);

        RewardGroup? GetRewardGroupInProgress(uint score);

        RewardGroup? GetLatestRewardGroupReceived(uint score);

        RewardGroup? GetLatestCompletedRewardGroup(uint score);
    }
}
