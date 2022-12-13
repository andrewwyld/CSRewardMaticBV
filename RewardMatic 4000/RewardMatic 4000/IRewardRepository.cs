#nullable enable

using System.Collections.Generic;

namespace RewardMatic_4000
{
    public interface IRewardRepository
	{
		IEnumerable<Reward> GetRewards();

		Reward? GetReward(string name);

		IEnumerable<RewardGroup> GetRewardGroups();

		RewardGroup? GetRewardGroup(string name);
	}
}
