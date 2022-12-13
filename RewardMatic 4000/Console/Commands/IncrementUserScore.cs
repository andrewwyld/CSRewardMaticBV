using System;
using RewardMatic_4000;

namespace ConsoleApp.Commands
{
	public class IncrementUserScoreInput
	{
		public User User { get; set; }
		public uint Score { get; set; }
	}
    public class IncrementUserScore : ICommand<IncrementUserScoreInput, User>
    {
        private readonly IUserRepository userRepository;

        public IncrementUserScore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string Name => "IncrementUserScore";

        public string Prompt => "Enter the name of the user and then points you wish to add (separated with a space)";

        public IncrementUserScoreInput ParseUserInput(string input)
        {
            var splitInput = input.Split(' ');
            if (splitInput.Count() < 2)
            {
                throw new Exception("Please enter only a name and a score separated by space");
            }

            uint score;
            if (!uint.TryParse(splitInput[1], out score))
            {
                throw new Exception("Please enter a non-negative integer for a score");
            }

            var user = userRepository.GetUser(splitInput[0]);

            if (user is null)
            {
                throw new Exception($"No user with name {splitInput[0]} found :(");
            }

            return new IncrementUserScoreInput
            {
                User = user,
                Score = score
            };
        }

        public User? Execute(IncrementUserScoreInput input)
        {
            input.User.UpdateScore(input.Score);
            return input.User;
        }

        public string FormatOutput(User output)
        {
            return $"User {output.Name}'s score is now {output.Score}! Check the awards he has by running ViewUserRewards";
        }
    }
}

