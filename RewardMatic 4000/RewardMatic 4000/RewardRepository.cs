using System.Collections.Generic;
using System.Linq;

namespace RewardMatic_4000
{
    public class RewardRepository : IRewardRepository
    {
        private readonly IEnumerable<Reward> _availableRewards;

        public RewardRepository()
        {
            _availableRewards = Reward.AvailableRewards;
        }

        public RewardRepository(List<Reward> rewards)
        {
            if (rewards is null)
            {
                _availableRewards = new List<Reward>();
            }
            else
            {
                _availableRewards = rewards;
            }
        }

        public Reward GetCurrentReward(uint score)
        {
            return _availableRewards
                .Where(reward => reward.ScoreDifferential <= score)
                .LastOrDefault();
        }

        public Reward GetRewardInProgress(uint score)
        {
            return  _availableRewards
                .Where(reward => reward.ScoreDifferential > score)
                .FirstOrDefault();
        }
    }
}

