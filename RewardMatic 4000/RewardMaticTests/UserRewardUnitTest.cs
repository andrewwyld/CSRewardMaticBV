using NUnit.Framework;
using System.Collections.Generic;

namespace RewardMatic_4000
{
    public class UserTest
    {
        private IRewardService _rewardService;

        [SetUp]
        public void Setup()
        {
            _rewardService = new RewardService(new RewardRepository(new List<Reward>()));
        }

        [Test]
        public void TestScoreIncrementsCorrectly()
        {
            var user = new User("Rodrigo", _rewardService);

            Assert.That(user.Score, Is.EqualTo(0));

            user.UpdateScore(250);
            
            Assert.That(user.Score, Is.EqualTo(250));

            user.UpdateScore(250000);
            
            Assert.That(user.Score, Is.EqualTo(250250));
        }
    }
}