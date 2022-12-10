#nullable enable

using System.Collections.Generic;

namespace RewardMatic_4000
{
	public interface IRewardRepository
	{
		IEnumerable<Reward> GetRewards();

		Reward? GetRewardInProgress(uint score);

		Reward? GetLatestRewardReceived(uint score);
	}
}
