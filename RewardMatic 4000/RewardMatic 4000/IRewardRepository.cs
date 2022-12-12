#nullable enable

using System.Collections.Generic;

namespace RewardMatic_4000
{
    public record RewardWithGroup(Reward reward, string? groupName = null);

    public interface IRewardRepository
	{
		IEnumerable<RewardWithGroup> GetRewards();

		Reward? GetReward(string name);

		IEnumerable<RewardGroup> GetRewardGroups();

		RewardGroup? GetRewardGroup(string name);

	}
}
