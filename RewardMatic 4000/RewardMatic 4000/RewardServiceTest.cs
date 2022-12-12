using System;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;

namespace RewardMatic_4000
{
	public class RewardServiceTest
	{
        List<Reward> _rewards;

        List<RewardGroup> _rewardGroups;

        List<RewardWithGroup> _rewardsWithGroups;

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

            _rewardsWithGroups = new List<RewardWithGroup>
            {
                new RewardWithGroup(new Reward(1, "first-group-first"), "first-group"),
                new RewardWithGroup(new Reward(2, "first-group-third"), "first-group"),
                new RewardWithGroup(new Reward(2, "second-group-first"), "second-group"),
                new RewardWithGroup(new Reward(2, "third-group-third"), "third-group"),
                new RewardWithGroup(new Reward(3, "second-group-third"), "second-group"),
                new RewardWithGroup(new Reward(4, "first-group-second"), "first-group"),
                new RewardWithGroup(new Reward(5, "third-group-fourth"), "third-group"),
                new RewardWithGroup(new Reward(6, "second-group-second"), "second-group"),
                new RewardWithGroup(new Reward(7, "third-group-second"), "third-group"),
                new RewardWithGroup(new Reward(10, "third-group-first"), "third-group")
            };
        }

        // test to make sure the "reward in progress" function works correctly
        [Test]
        public void TestRewardInProgress()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewardsWithGroups);

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            Assert.IsNotNull(service.GetRewardInProgress(score));

            Assert.AreEqual(service.GetRewardInProgress(score).Name, "first-group-first");
            Assert.AreEqual(service.GetRewardInProgress(score).ScoreDifferential, 1);

            score = 1;

            Assert.IsNotNull(service.GetRewardInProgress(score));
            Assert.AreEqual(service.GetRewardInProgress(score).Name, "first-group-third");
            Assert.AreEqual(service.GetRewardInProgress(score).ScoreDifferential, 2);

            score = 20;
            Assert.IsNull(service.GetRewardInProgress(score));


            var emptyRepository = new RewardRepository(new List<Reward>());
            service = new RewardService(emptyRepository);
            score = 0;
            Assert.IsNull(service.GetRewardInProgress(score));

            score = 2500000;
            Assert.IsNull(service.GetRewardInProgress(score));
        }

        // test to make sure the "latest reward received" function works correctly
        [Test]
        public void TestLatestReward()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewardsWithGroups);

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            Assert.IsNull(service.GetLatestRewardReceived(score));

            score = 1;

            Assert.IsNotNull(service.GetLatestRewardReceived(score));
            Assert.AreEqual("first-group-first", service.GetLatestRewardReceived(score).Name);
            Assert.AreEqual(1, service.GetLatestRewardReceived(score).ScoreDifferential);

            score = 5;

            Assert.IsNotNull(service.GetLatestRewardReceived(score));
            Assert.AreEqual("third-group-fourth", service.GetLatestRewardReceived(score).Name);
            Assert.AreEqual(5, service.GetLatestRewardReceived(score).ScoreDifferential);

            score = 0;
            rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(new List<RewardWithGroup>());
            service = new RewardService(rewardRepositoryMock.Object);

            Assert.IsNull(service.GetLatestRewardReceived(score));

            score = 2500000;
            Assert.IsNull(service.GetLatestRewardReceived(score));
        }


        [Test]
        public void TestGetRewardGroupInProgress()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewardsWithGroups);

            var groupNames = new List<string>() { "first-group", "third-group", "second-group" };
            groupNames.ForEach(groupName =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroup(groupName)).Returns(
                    new RewardGroup(groupName, new List<Reward> { new Reward(1, "") }));
            });

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            // Base case where score is 0
            Assert.IsNotNull(service.GetRewardGroupInProgress(score));
            Assert.AreEqual("first-group", service.GetRewardGroupInProgress(score).Name);

            score = 5;

            // Score is somewhere inbetween the reward groups
            Assert.IsNotNull(service.GetRewardGroupInProgress(score));
            Assert.AreEqual("second-group", service.GetRewardGroupInProgress(score).Name);

            score = 6;
            Assert.IsNotNull(service.GetRewardGroupInProgress(score));
            Assert.AreEqual("third-group", service.GetRewardGroupInProgress(score).Name);

            score = 30;
            // Score is outside of the bounds of all the groups
            Assert.IsNull(service.GetRewardGroupInProgress(score));
        }

        [Test]
        public void TestGetLatestRewardGroup()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewardsWithGroups);

            var groupNames = new List<string>() { "first-group", "second-group", "third-group" };
            groupNames.ForEach(groupName =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroup(groupName)).Returns(
                    new RewardGroup(groupName, new List<Reward> { new Reward(1, "") }));
            });

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            // Base case where score is 0, no rewards completed
            Assert.IsNull(service.GetLatestRewardGroupReceived(score));

            score = 1;
            Assert.IsNotNull(service.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("first-group", service.GetLatestRewardGroupReceived(score).Name);


            // We have just completed a reward in the third group
            score = 5;
            Assert.IsNotNull(service.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("third-group", service.GetLatestRewardGroupReceived(score).Name);

            // We have just completed a reward in the second group
            score = 6;
            Assert.IsNotNull(service.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("second-group", service.GetLatestRewardGroupReceived(score).Name);

            // We have completed all the groups
            score = 60;
            Assert.IsNotNull(service.GetLatestRewardGroupReceived(score));
            Assert.AreEqual("third-group", service.GetLatestRewardGroupReceived(score).Name);
        }

        [Test]
        public void TestGetLatestCompletedRewardGroup()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            rewardRepositoryMock.Setup(repository => repository.GetRewardGroups()).Returns(_rewardGroups);

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            // Base case where score is 0. No reward groups completed
            Assert.IsNull(service.GetLatestCompletedRewardGroup(score));

            score = 1;
            Assert.IsNull(service.GetLatestCompletedRewardGroup(score));

            // We have just completed the first group
            score = 5;
            Assert.IsNotNull(service.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("first-group", service.GetLatestCompletedRewardGroup(score).Name);

            // We have just completed a reward in the second group
            score = 6;
            Assert.IsNotNull(service.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("second-group", service.GetLatestCompletedRewardGroup(score).Name);

            // We have completed all the groups
            score = 60;
            Assert.IsNotNull(service.GetLatestCompletedRewardGroup(score));
            Assert.AreEqual("third-group", service.GetLatestCompletedRewardGroup(score).Name);
        }
    }
}

