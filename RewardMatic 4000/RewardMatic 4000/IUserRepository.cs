#nullable enable

using System;
using System.Collections.Generic;

namespace RewardMatic_4000
{
	public interface IUserRepository
	{
		IEnumerable<User> GetUsers();

		User? GetUser(string name);

		User? CreateUser(string name);
	}
}

