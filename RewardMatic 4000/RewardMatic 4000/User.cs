#nullable enable

namespace RewardMatic_4000
{
    public class User
    {
        private uint _score = 0;
        private readonly IRewardService _rewardService;

        public User(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

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
    }
}