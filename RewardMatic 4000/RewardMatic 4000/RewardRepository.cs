using System.Collections.Generic;
using System.Linq;

namespace RewardMatic_4000
{
    public class RewardRepository : IRewardRepository
    {
        private readonly List<Reward> _availableRewards;
        private readonly List<RewardGroup> rewardGroups;

        public RewardRepository(List<Reward> rewards)
        {
            if (rewards is null)
            {
                _availableRewards = new List<Reward>();
            }
            else
            {
                _availableRewards = rewards;
                _availableRewards
                    .Sort((reward1, reward2) => reward1.ScoreDifferential.CompareTo(reward2.ScoreDifferential));
            }
        }

        public RewardRepository(List<RewardGroup> rewardGroups)
        {
            this.rewardGroups = rewardGroups;
        }

        public RewardGroup GetLatestRewardGroupReceived(uint score)
        {
            throw new System.NotImplementedException();
        }

        public Reward GetLatestRewardReceived(uint score)
        {
            return _availableRewards
                .Where(reward => reward.ScoreDifferential <= score)
                .LastOrDefault();
        }

        public RewardGroup GetRewardGroupInProgress(uint score)
        {
            throw new System.NotImplementedException();
        }

        public Reward GetRewardInProgress(uint score)
        {
            return  _availableRewards
                .Where(reward => reward.ScoreDifferential > score)
                .FirstOrDefault();
        }

        public IEnumerable<Reward> GetRewards()
        {
            return _availableRewards;
        }
    }
}
