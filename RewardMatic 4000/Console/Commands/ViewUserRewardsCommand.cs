using System;
using System.Text;
using RewardMatic_4000;

namespace ConsoleApp.Commands
{
	public class ViewUserRewardsInput
	{
		public string Username { get; set; }

		public string RewardsQuery { get; set; }
	}

	public class ViewUserRewardsOutput
	{
        public User User { get; set; }

        public Reward? RewardInProgress { get; set; }

		public Reward? LatestRewardAchieved { get; set; }

		public RewardGroup? RewardGroupInProgress { get; set; }

		public RewardGroup? LatestRewardAchievedRewardGroup { get; set; }

		public RewardGroup? LatestCompletedRewardGroup { get; set; }
	}
    public class ViewUserRewardsCommand : ICommand<ViewUserRewardsInput, ViewUserRewardsOutput>
    {
        private readonly IUserRepository userRepository;

        public ViewUserRewardsCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string Prompt => "Enter the username and then the rewards query separated by a space. It accepts following fields (multiple fields are separated by a space)" +
            "RewardInProgress, LatestRewardAchieved, RewardGroupInProgress, LatestRewardAchievedRewardGroup, LatestCompletedRewardGroup. Type All if you'd like to see all properties";

        public string Name => "ViewUserRewards";

        public ViewUserRewardsOutput? Execute(ViewUserRewardsInput input)
        {
            var output = new ViewUserRewardsOutput();

            var user = userRepository.GetUser(input.Username);

            if (user is null)
            {
                throw new Exception("User with this name cannot be found");
            }

            output.User = user;

            if (input.RewardsQuery.Contains("RewardInProgress") || input.RewardsQuery.Contains("All"))
            {
                output.RewardInProgress = user.GetRewardInProgress();
            }
            if (input.RewardsQuery.Contains("LatestRewardAchieved") || input.RewardsQuery.Contains("All"))
            {
                output.LatestRewardAchieved = user.GetLatestRewardReceived();
            }
            if (input.RewardsQuery.Contains("RewardGroupInProgress") || input.RewardsQuery.Contains("All"))
            {
                output.RewardGroupInProgress = user.GetRewardGroupInProgress();
            }
            if (input.RewardsQuery.Contains("LatestRewardAchievedRewardGroup") || input.RewardsQuery.Contains("All"))
            {
                output.LatestRewardAchievedRewardGroup = user.GetLatestRewardGroupReceived();
            }
            if (input.RewardsQuery.Contains("LatestCompletedRewardGroup") || input.RewardsQuery.Contains("All"))
            {
                output.LatestCompletedRewardGroup = user.GetLatestCompletedRewardGroup();
            }
            return output;
        }

        public string FormatOutput(ViewUserRewardsOutput output)
        {
            var sb = new StringBuilder("");

            sb.Append($"User {output.User.Name} with score {output.User.Score} has the following awards:");
            sb.Append(Environment.NewLine);

            if (output.RewardInProgress is not null)
            {
                sb.Append($"----Reward in progress is: {output.RewardInProgress?.Name}");
                sb.Append(Environment.NewLine);
            }
            if (output.LatestRewardAchieved is not null)
            {
                sb.Append($"----Latest reward achieved is: {output.LatestRewardAchieved?.Name}");
                sb.Append(Environment.NewLine);
            }
            if (output.LatestRewardAchieved is not null)
            {
                sb.Append($"----Reward group for the reward in progress is: {output.RewardGroupInProgress?.Name}");
                sb.Append(Environment.NewLine);
            }
            if (output.LatestRewardAchievedRewardGroup is not null)
            {
                sb.Append($"----Reward group for the latest achieved award is: {output.LatestRewardAchievedRewardGroup?.Name}");
                sb.Append(Environment.NewLine);
            }
            if (output.LatestCompletedRewardGroup is not null)
            {
                sb.Append($"----Latest fully completed reward group: {output.LatestCompletedRewardGroup?.Name}");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public ViewUserRewardsInput ParseUserInput(string input)
        {
            var trimmed = input.Trim();
            var splitInput = trimmed.Split(' ');

            return new ViewUserRewardsInput
            {
                Username = splitInput[0],
                RewardsQuery = string.Join(" ", splitInput.Skip(1))
            };
        }
    }
}

