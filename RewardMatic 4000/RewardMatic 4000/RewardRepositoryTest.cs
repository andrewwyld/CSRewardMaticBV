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

            Assert.IsNotEmpty(rewards);
            Assert.AreEqual(4, rewards.Count());

            var isOrdered = true;

            for (int i = 0; i < rewards.Count() - 1; i++)
            {
                if (rewards[i].reward.ScoreDifferential > rewards[i + 1].reward.ScoreDifferential)
                {
                    isOrdered = false;
                    break;
                }
            }

            Assert.IsTrue(isOrdered);

            // Test with an empty list of rewards
            repository = new RewardRepository(new List<Reward>());

            Assert.IsEmpty(repository.GetRewards());


            // Test by creating the repository with a list of reward groups
            repository = new RewardRepository(_rewardGroups);

            rewards = repository.GetRewards().ToList();

            Assert.IsNotEmpty(rewards);
            Assert.AreEqual(10, rewards.Count());

            isOrdered = true;

            for (int i = 0; i < rewards.Count() - 1; i++)
            {
                if (rewards[i].reward.ScoreDifferential > rewards[i + 1].reward.ScoreDifferential)
                {
                    isOrdered = false;
                    break;
                }
            }

            Assert.IsTrue(isOrdered);

            // Test with an empty list of reward groups
            repository = new RewardRepository(new List<RewardGroup>());

            Assert.IsEmpty(repository.GetRewards());
        }

        [Test]
        public void TestGetReward()
        {
            // Test by creating with a list of rewards
            var repository = new RewardRepository(_rewards);

            Assert.IsNotNull(repository.GetReward("first"));
            Assert.AreEqual(1, repository.GetReward("first").ScoreDifferential);
            Assert.AreEqual("first", repository.GetReward("first").Name);

            Assert.IsNull(repository.GetReward("non-existing-award"));

            // Test by creating with a list of reward groups
            repository = new RewardRepository(_rewardGroups);

            Assert.IsNotNull(repository.GetReward("first-group-second"));
            Assert.AreEqual(4, repository.GetReward("first-group-second").ScoreDifferential);
            Assert.AreEqual("first-group-second", repository.GetReward("first-group-second").Name);

            Assert.IsNull(repository.GetReward("non-existing-group-non-existent"));
        }

        [Test]
        public void TestGetRewardGroups()
        {
            // Test by creating the repository with reward groups
            var repository = new RewardRepository(_rewardGroups);

            var rewardGroups = repository.GetRewardGroups().ToList();

            Assert.IsNotEmpty(rewardGroups);
            Assert.AreEqual(3, rewardGroups.Count());

            // Test by creating the repository with plain rewards
            repository = new RewardRepository(_rewards);

            Assert.IsEmpty(repository.GetRewardGroups());
        }

        [Test]
        public void GetRewardGroup()
        {
            // Test by creating the repository with reward groups
            var repository = new RewardRepository(_rewardGroups);

            var rewardGroup = repository.GetRewardGroup("first-group");

            Assert.IsNotNull(rewardGroup);
            Assert.AreEqual(3, rewardGroup.GroupRewards.Count());

            // Test by creating the repository with plain rewards
            repository = new RewardRepository(_rewards);

            Assert.IsNull(repository.GetRewardGroup("first-group"));
        }
    }
}

