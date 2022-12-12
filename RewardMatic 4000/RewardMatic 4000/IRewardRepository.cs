#nullable enable

using System.Collections.Generic;

namespace RewardMatic_4000
{
	public interface IRewardRepository
	{
		IEnumerable<Reward> GetRewards();

		Reward? GetRewardInProgress(uint score);

		Reward? GetLatestRewardReceived(uint score);

		RewardGroup? GetRewardGroupInProgress(uint score);

		RewardGroup? GetLatestRewardGroupReceived(uint score);

		RewardGroup? GetLatestCompletedRewardGroup(uint score);
	}
}
