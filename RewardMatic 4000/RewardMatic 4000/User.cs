#nullable enable

namespace RewardMatic_4000
{
    public class User
    {
        private uint _score = 0;
        private readonly IRewardService _rewardService;

        public User(string name, IRewardService rewardService)
        {
            _rewardService = rewardService;
            Name = name;
        }

        public string Name { get; set; }

        public uint Score
        {
            get { return _score; }
        }

        /// <summary>
        /// Method <c>UpdateScore</c> increments the user's score by the given nonnegative input
        /// </summary>
        public void UpdateScore(uint update)
        {
            _score += update;
        }

        public Reward? GetRewardInProgress() =>
            _rewardService.GetRewardInProgress(_score);

        public Reward? GetLatestRewardReceived() =>
            _rewardService.GetLatestRewardReceived(_score);

        public RewardGroup? GetRewardGroupInProgress() =>
            _rewardService.GetRewardGroupInProgress(_score);

        public RewardGroup? GetLatestRewardGroupReceived() =>
            _rewardService.GetLatestRewardGroupReceived(_score);

        public RewardGroup? GetLatestCompletedRewardGroup() =>
            _rewardService.GetLatestCompletedRewardGroup(_score);
    }
}