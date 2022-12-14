using System;
using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using static System.Formats.Asn1.AsnWriter;

namespace RewardMatic_4000
{
	public class RewardServiceTest
	{
        List<RewardGroup> _rewardGroups;

        List<Reward> _rewards;

        [SetUp]
        public void Setup()
        {
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

            _rewards = new List<Reward>
            {
                new Reward(1, "first-group-first", 1, _rewardGroups[0]),
                new Reward(4, "first-group-second", 5, _rewardGroups[0]),
                new Reward(2, "first-group-third", 7, _rewardGroups[0]),
                new Reward(2, "second-group-first", 9, _rewardGroups[1]),
                new Reward(6, "second-group-second", 15, _rewardGroups[1]),
                new Reward(3, "second-group-third", 18,  _rewardGroups[1]),
                new Reward(10, "third-group-first", 28, _rewardGroups[2]),
                new Reward(7, "third-group-second", 35, _rewardGroups[2]),
                new Reward(2, "third-group-third", 37, _rewardGroups[2]),
                new Reward(5, "third-group-fourth", 42, _rewardGroups[2]),
            };
        }

        // test to make sure the "reward in progress" function works correctly
        [Test]
        public void TestRewardInProgress()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            var WithNonEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewards);
                Assert.That(service.GetRewardInProgress(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetRewardInProgress(score).Name, Is.EqualTo("first-group-first"));
                    Assert.That(service.GetRewardInProgress(score).ScoreDifferential, Is.EqualTo(1));
                });
                score = 1;

                Assert.That(service.GetRewardInProgress(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetRewardInProgress(score).Name, Is.EqualTo("first-group-second"));
                    Assert.That(service.GetRewardInProgress(score).ScoreDifferential, Is.EqualTo(4));
                });
                score = 20;

                Assert.That(service.GetRewardInProgress(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetRewardInProgress(score).Name, Is.EqualTo("third-group-first"));
                    Assert.That(service.GetRewardInProgress(score).ScoreDifferential, Is.EqualTo(10));
                });
                score = 100;
                Assert.That(service.GetRewardInProgress(score), Is.Null);
            };

            var WithEmptyRewardRepository = () =>
            {
                // Stub an empty reward repository
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(new List<Reward>());
                service = new RewardService(rewardRepositoryMock.Object);

                score = 0;
                Assert.That(service.GetRewardInProgress(score), Is.Null);

                score = 2500000;
                Assert.That(service.GetRewardInProgress(score), Is.Null);
            };

            WithNonEmptyRewardRepository();
            WithEmptyRewardRepository();
        }

        [Test]
        public void TestLatestReward()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();
            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            var WithNonEmptyRewardRepository = () =>
            {
                // Non-empty reward repository
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewards);


                Assert.That(service.GetLatestRewardReceived(score), Is.Null);

                score = 1;

                Assert.That(service.GetLatestRewardReceived(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetLatestRewardReceived(score).Name, Is.EqualTo("first-group-first"));
                    Assert.That(service.GetLatestRewardReceived(score).ScoreDifferential, Is.EqualTo(1));
                });
                score = 7;

                Assert.That(service.GetLatestRewardReceived(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetLatestRewardReceived(score).Name, Is.EqualTo("first-group-third"));
                    Assert.That(service.GetLatestRewardReceived(score).ScoreDifferential, Is.EqualTo(2));
                });
                score = 100;
                Assert.That(service.GetLatestRewardReceived(score), Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(service.GetLatestRewardReceived(score).Name, Is.EqualTo("third-group-fourth"));
                    Assert.That(service.GetLatestRewardReceived(score).ScoreDifferential, Is.EqualTo(5));
                });
            };

            var WithEmptyRewardRepository = () =>
            {
                // Empty reward repository
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(new List<Reward>());
                service = new RewardService(rewardRepositoryMock.Object);

                score = 0;
                Assert.That(service.GetLatestRewardReceived(score), Is.Null);

                score = 2500000;
                Assert.That(service.GetLatestRewardReceived(score), Is.Null);
            };

            WithNonEmptyRewardRepository();
            WithEmptyRewardRepository();
        }

        [Test]
        public void TestGetRewardGroupInProgress()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();

            var groupNames = new List<string>() { "first-group", "third-group", "second-group" };
            groupNames.ForEach(groupName =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroup(groupName)).Returns(
                    new RewardGroup(groupName, new List<Reward> { new Reward(1, "") }));
            });

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            var WithNonEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewards);

                // Base case where score is 0
                Assert.That(service.GetRewardGroupInProgress(score), Is.Not.Null);
                Assert.That(service.GetRewardGroupInProgress(score).Name, Is.EqualTo("first-group"));

                score = 7;

                // Score is somewhere inbetween the reward groups
                Assert.That(service.GetRewardGroupInProgress(score), Is.Not.Null);
                Assert.That(service.GetRewardGroupInProgress(score).Name, Is.EqualTo("second-group"));

                score = 18;
                Assert.That(service.GetRewardGroupInProgress(score), Is.Not.Null);
                Assert.That(service.GetRewardGroupInProgress(score).Name, Is.EqualTo("third-group"));

                score = 60;
                // Score is outside of the bounds of all the groups
                Assert.That(service.GetRewardGroupInProgress(score), Is.Null);
            };

            var WithEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(new List<Reward>());

                Assert.That(service.GetRewardGroupInProgress(score), Is.Null);
            };

            WithNonEmptyRewardRepository();
            WithEmptyRewardRepository();

        }

        [Test]
        public void TestGetLatestRewardGroup()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();

            var groupNames = new List<string>() { "first-group", "second-group", "third-group" };
            groupNames.ForEach(groupName =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroup(groupName)).Returns(
                    new RewardGroup(groupName, new List<Reward> { new Reward(1, "") }));
            });

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            var WithNonEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(_rewards);

                // Base case where score is 0, no rewards completed
                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Null);

                score = 1;
                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Not.Null);
                Assert.That(service.GetLatestRewardGroupReceived(score).Name, Is.EqualTo("first-group"));


                // We have just completed a reward in the third group
                score = 9;
                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Not.Null);
                Assert.That(service.GetLatestRewardGroupReceived(score).Name, Is.EqualTo("second-group"));

                // We have just completed a reward in the second group
                score = 28;
                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Not.Null);
                Assert.That(service.GetLatestRewardGroupReceived(score).Name, Is.EqualTo("third-group"));

                // We have completed all the groups
                score = 60;
                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Not.Null);
                Assert.That(service.GetLatestRewardGroupReceived(score).Name, Is.EqualTo("third-group"));
            };

            var WithEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewards()).Returns(new List<Reward>());

                Assert.That(service.GetLatestRewardGroupReceived(score), Is.Null);
            };

            WithNonEmptyRewardRepository();
            WithEmptyRewardRepository();
        }

        [Test]
        public void TestGetLatestCompletedRewardGroup()
        {
            var rewardRepositoryMock = new Mock<IRewardRepository>();

            var service = new RewardService(rewardRepositoryMock.Object);
            uint score = 0;

            var WithNonEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroups()).Returns(_rewardGroups);

                // Base case where score is 0. No reward groups completed
                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Null);

                score = 1;
                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Null);

                // We have just completed the first group
                score = 7;
                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Not.Null);
                Assert.That(service.GetLatestCompletedRewardGroup(score).Name, Is.EqualTo("first-group"));

                // We have just completed a reward in the second group
                score = 18;
                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Not.Null);
                Assert.That(service.GetLatestCompletedRewardGroup(score).Name, Is.EqualTo("second-group"));

                // We have completed all the groups
                score = 60;
                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Not.Null);
                Assert.That(service.GetLatestCompletedRewardGroup(score).Name, Is.EqualTo("third-group"));
            };

            var WithEmptyRewardRepository = () =>
            {
                rewardRepositoryMock.Setup(repository => repository.GetRewardGroups()).Returns(new List<RewardGroup>());

                Assert.That(service.GetLatestCompletedRewardGroup(score), Is.Null);
            };

            WithNonEmptyRewardRepository();
            WithEmptyRewardRepository();
        }
    }
}

