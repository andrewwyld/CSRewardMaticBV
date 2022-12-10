using NUnit.Framework;
using System.Collections.Generic;

namespace RewardMatic_4000
{
    public class UserTest
    {
        private IRewardRepository _rewardRepository;

        [SetUp]
        public void Setup()
        {
            _rewardRepository = new RewardRepository(new List<Reward>());
        }

        // test to make sure a user's score updates correctly and is arithmetically consistent
        [Test]
        public void TestScoreIncrementsCorrectly()
        {
            var user = new User(_rewardRepository);

            Assert.AreEqual(0, user.Score);

            user.UpdateScore(250);
            
            Assert.AreEqual(250, user.Score);

            user.UpdateScore(250000);
            
            Assert.AreEqual(250250, user.Score);
        }
    }
}