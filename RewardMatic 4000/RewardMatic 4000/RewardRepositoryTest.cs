using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace RewardMatic_4000
{
	public class RewardRepositoryTest
	{
        List<Reward> _rewards;

        [SetUp]
        public void Setup()
        {
            _rewards = new List<Reward>
            {
                new Reward(1, "first"),
                new Reward(4, "fourth"),
                new Reward(2, "second"),
                new Reward(3, "third")
            };
        }

        // test to make sure the "reward in progress" function works correctly
        [Test]
        public void TestRewardInProgress()
        {
            var rewardRepository = new RewardRepository(_rewards);
            uint score = 0;

            Assert.IsNotNull(rewardRepository.GetRewardInProgress(score));

            Assert.AreEqual(rewardRepository.GetRewardInProgress(score).Message, "first");
            Assert.AreEqual(rewardRepository.GetRewardInProgress(score).ScoreDifferential, 1);

            score = 1;

            Assert.IsNotNull(rewardRepository.GetRewardInProgress(score));
            Assert.AreEqual(rewardRepository.GetRewardInProgress(score).Message, "second");
            Assert.AreEqual(rewardRepository.GetRewardInProgress(score).ScoreDifferential, 2);

            score = 4;
            Assert.IsNull(rewardRepository.GetRewardInProgress(score));


            var emptyRepository = new RewardRepository(new List<Reward>());
            score = 0;
            Assert.IsNull(emptyRepository.GetRewardInProgress(score));

            score = 2500000;
            Assert.IsNull(emptyRepository.GetRewardInProgress(score));
        }

        // test to make sure the "latest reward received" function works correctly
        [Test]
        public void TestLatestReward()
        {
            var rewardRepository = new RewardRepository(_rewards);
            uint score = 0;

            Assert.IsNull(rewardRepository.GetLatestRewardReceived(score));

            score = 1;

            Assert.IsNotNull(rewardRepository.GetLatestRewardReceived(score));
            Assert.AreEqual("first", rewardRepository.GetLatestRewardReceived(score).Message);
            Assert.AreEqual(1, rewardRepository.GetLatestRewardReceived(score).ScoreDifferential);

            score = 5;

            Assert.IsNotNull(rewardRepository.GetLatestRewardReceived(score));
            Assert.AreEqual("fourth", rewardRepository.GetLatestRewardReceived(score).Message);
            Assert.AreEqual(4, rewardRepository.GetLatestRewardReceived(score).ScoreDifferential);

            score = 0;
            var emptyRepository = new RewardRepository(new List<Reward>());
            Assert.IsNull(emptyRepository.GetLatestRewardReceived(score));

            score = 2500000;
            Assert.IsNull(emptyRepository.GetLatestRewardReceived(score));
        }
    }
}

