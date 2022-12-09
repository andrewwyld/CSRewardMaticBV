
using System;
using System.Collections.Generic;

namespace RewardMatic_4000
{
	public interface IRewardRepository
	{
		Reward? GetRewardInProgress(uint score);

		Reward? GetCurrentReward(uint score);
	}
}
