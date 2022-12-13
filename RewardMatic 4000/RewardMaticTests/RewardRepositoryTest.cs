using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

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

        [Test]
        public void TestGetRewards()
        {
            // Test by creating the repository with a list of rewards
            var repository = new RewardRepository(_rewards);

            var rewards = repository.GetRewards().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(rewards, Has.Count.EqualTo(4));
                Assert.That(rewards, Is.Not.Empty);
                Assert.That(rewards.First().Name, Is.EqualTo("first"));
                Assert.That(rewards.Skip(1).First().Name, Is.EqualTo("fourth"));
            });

            // Test with an empty list of rewards
            repository = new RewardRepository(new List<Reward>());

            Assert.That(repository.GetRewards(), Is.Empty);


            // Test by creating the repository with a list of reward groups
            repository = new RewardRepository(_rewardGroups);

            rewards = repository.GetRewards().ToList();

            Assert.That(rewards, Is.Not.Empty);
            Assert.That(rewards, Has.Count.EqualTo(10));

            // Test with an empty list of reward groups
            repository = new RewardRepository(new List<RewardGroup>());

            Assert.That(repository.GetRewards(), Is.Empty);
        }

        [Test]
        public void TestGetReward()
        {
            // Test by creating with a list of rewards
            RewardRepository repository = new RewardRepository(_rewards);

            Assert.Multiple(() =>
            {
                Assert.That(repository.GetReward("first"), Is.Not.Null);
                Assert.That(repository.GetReward("first")?.ScoreDifferential, Is.EqualTo(1));
                Assert.That(repository.GetReward("first")?.Name, Is.EqualTo("first"));

                Assert.That(repository.GetReward("non-existing-award"), Is.Null);
            });

            // Test by creating with a list of reward groups
            repository = new RewardRepository(_rewardGroups);

            Assert.Multiple(() =>
            {
                Assert.That(repository.GetReward("first-group-second"), Is.Not.Null);
                Assert.That(repository.GetReward("first-group-second")?.ScoreDifferential, Is.EqualTo(4));
                Assert.That(repository.GetReward("first-group-second")?.Name, Is.EqualTo("first-group-second"));
                Assert.That(repository.GetReward("non-existing-group-non-existent"), Is.Null);
            });
        }

        [Test]
        public void TestGetRewardGroups()
        {
            // Test by creating the repository with reward groups
            var repository = new RewardRepository(_rewardGroups);

            var rewardGroups = repository.GetRewardGroups().ToList();

            Assert.That(rewardGroups, Is.Not.Empty);
            Assert.That(rewardGroups, Has.Count.EqualTo(3));

            // Test by creating the repository with plain rewards
            repository = new RewardRepository(_rewards);

            Assert.That(repository.GetRewardGroups(), Is.Empty);
        }

        [Test]
        public void GetRewardGroup()
        {
            // Test by creating the repository with reward groups
            var repository = new RewardRepository(_rewardGroups);

            var rewardGroup = repository.GetRewardGroup("first-group");

            Assert.That(rewardGroup, Is.Not.Null);
            Assert.That(rewardGroup.Rewards, Has.Count.EqualTo(3));

            // Test by creating the repository with plain rewards
            repository = new RewardRepository(_rewards);

            Assert.That(repository.GetRewardGroup("first-group"), Is.Null);
        }
    }
}

