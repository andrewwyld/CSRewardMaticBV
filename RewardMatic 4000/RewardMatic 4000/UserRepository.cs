#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace RewardMatic_4000
{
	public class UserRepository: IUserRepository
	{
        private readonly IRewardService rewardService;
        private readonly List<User> users = new List<User>();

        public UserRepository(IRewardService rewardService)
		{
            this.rewardService = rewardService;
        }

        public User CreateUser(string name)
        {
            var newUser = new User(name, rewardService);
            users.Add(newUser);
            return newUser;
        }

        public User? GetUser(string name)
        {
            return users.FirstOrDefault(user => user.Name == name);
        }

        public IEnumerable<User> GetUsers()
        {
            return users;
        }
    }
}

