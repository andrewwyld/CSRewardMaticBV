using NUnit.Framework;
using System.Collections.Generic;

namespace RewardMatic_4000
{
    public class Tests
    {
        private IRewardRepository _rewardRepository;
        private IRewardRepository _emptyRewardRepository;

        [SetUp]
        public void Setup()
        {
            var rewards = new List<Reward>
            {
                new Reward(1, "first"),
                new Reward(2, "second"),
                new Reward(3, "third"),
                new Reward(4, "fourth")
            };

            _rewardRepository = new RewardRepository(rewards);
            _emptyRewardRepository = new RewardRepository(new List<Reward>());
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

        // test to make sure the "reward in progress" function works correctly
        [Test]
        public void TestRewardInProgress()
        {
            var user = new User(_rewardRepository);

            var rewardInProgress = user.GetRewardInProgress();

            Assert.IsNotNull(user.GetRewardInProgress());
            
            Assert.AreEqual(user.GetRewardInProgress().Message, "first");
            Assert.AreEqual(user.GetRewardInProgress().ScoreDifferential, 1);

            user.UpdateScore(1);

            Assert.IsNotNull(user.GetRewardInProgress());
            Assert.AreEqual(user.GetRewardInProgress().Message, "second");
            Assert.AreEqual(user.GetRewardInProgress().ScoreDifferential, 2);

            user.UpdateScore(3);
            Assert.IsNull(user.GetRewardInProgress());


            var user2 = new User(_emptyRewardRepository);
            Assert.IsNull(user2.GetRewardInProgress());

            user2.UpdateScore(2500000);
            Assert.IsNull(user2.GetRewardInProgress());
        }

        // test to make sure the "latest reward received" function works correctly
        [Test]
        public void TestLatestReward()
        {
            var user = new User(_rewardRepository);
            
            Assert.IsNull(user.GetLatestRewardReceived());

            user.UpdateScore(1);

            Assert.IsNotNull(user.GetLatestRewardReceived());
            Assert.AreEqual("first", user.GetLatestRewardReceived().Message);
            Assert.AreEqual(1, user.GetLatestRewardReceived().ScoreDifferential);

            user.UpdateScore(5);

            Assert.IsNotNull(user.GetLatestRewardReceived());
            Assert.AreEqual("fourth", user.GetLatestRewardReceived().Message);
            Assert.AreEqual(4, user.GetLatestRewardReceived().ScoreDifferential);

            var user2 = new User(_emptyRewardRepository);
            Assert.IsNull(user2.GetLatestRewardReceived());

            user2.UpdateScore(2500000);
            Assert.IsNull(user2.GetLatestRewardReceived());
        }
    }
}