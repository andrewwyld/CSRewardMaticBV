using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace RewardMatic_4000
{
	public class RewardRepositoryTest
	{
        List<Reward> _rewards;

        List<RewardGroup> _rewardGroups;

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

            _rewardGroups = new List<RewardGroup>
            {
                new RewardGroup("first-group", new List<Reward>
                {
                    new Reward(1, "first-group-first"),
                    new Reward(4, "first-group-second"),
                    new Reward(2, "first-group-third")
                }),
                new RewardGroup("second-group", new List<Reward>
                {
                    new Reward(2, "second-group-first"),
                    new Reward(6, "second-group-second"),
                    new Reward(3, "second-group-third")
                }),
                new RewardGroup("third-group", new List<Reward>
                {
                    new Reward(10, "third-group-first"),
                    new Reward(7, "third-group-second"),
                    new Reward(2, "third-group-third"),
                    new Reward(5, "third-group-fourth")
                })
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


        [Test]
        public void TestGetRewardGroupInProgress()
        {
            var repository = new RewardRepository(_rewardGroups);
            uint score = 0;

            // Base case where score is 0
            Assert.IsNotNull(repository.GetRewardGroupInProgress(score));
            Assert.AreEqual("first-group", repository.GetRewardGroupInProgress(score).Name);

            score = 5;

            // Score is somewhere inbetween the reward groups
            Assert.IsNotNull(repository.GetRewardGroupInProgress(score));
            Assert.AreEqual("second-group", repository.GetRewardGroupInProgress(score).Name);

            score = 6;
            Assert.IsNotNull(repository.GetRewardGroupInProgress(score));
            Assert.AreEqual("third-group", repository.GetRewardGroupInProgress(score).Name);

            score = 30;
            // Score is outside of the bounds of all the groups
            Assert.IsNull(repository.GetRewardGroupInProgress(score));
        }

        [Test]
        public void TestGetLatestRewardGroup()
        {
            var repository = new RewardRepository(_rewardGroups);
            uint score = 0;

            // Base case where score is 0, no rewards completed
            Assert.IsNull(repository.GetLatestRewardGroupReceived(score));

            score = 1;
            Assert.IsNotNull(repository.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("first-group", repository.GetLatestRewardGroupReceived(score).Name);


            // We have just completed a reward in the third group
            score = 5;
            Assert.IsNotNull(repository.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("third-group", repository.GetLatestRewardGroupReceived(score).Name);

            // We have just completed a reward in the second group
            score = 6;
            Assert.IsNotNull(repository.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("second-group", repository.GetLatestRewardGroupReceived(score).Name);

            // We have completed all the groups
            score = 60;
            Assert.IsNotNull(repository.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("third-group", repository.GetLatestRewardGroupReceived(score).Name);
        }

        [Test]
        public void TestGetLatestCompletedRewardGroup()
        {
            var repository = new RewardRepository(_rewardGroups);
            uint score = 0;

            // Base case where score is 0. No reward groups completed
            Assert.IsNull(repository.GetLatestCompletedRewardGroup(score));

            score = 1;
            Assert.IsNull(repository.GetLatestCompletedRewardGroup(score));

            // We have just completed the first group
            score = 5;
            Assert.IsNotNull(repository.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("first-group", repository.GetLatestCompletedRewardGroup(score).Name);

            // We have just completed a reward in the second group
            score = 6;
            Assert.IsNotNull(repository.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("second-group", repository.GetLatestCompletedRewardGroup(score).Name);

            // We have completed all the groups
            score = 60;
            Assert.IsNotNull(repository.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("third-group", repository.GetLatestCompletedRewardGroup(score).Name);
        }
    }
}

