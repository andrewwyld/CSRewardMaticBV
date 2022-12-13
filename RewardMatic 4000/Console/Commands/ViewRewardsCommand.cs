using System;
using System.Linq;
using System.Text;
using RewardMatic_4000;

namespace ConsoleApp.Commands
{
    public class ViewRewardsCommand : ICommand<string, List<RewardGroup>>
    {
        private readonly IRewardRepository rewardRepository;

        public ViewRewardsCommand(IRewardRepository rewardRepository)
        {
            this.rewardRepository = rewardRepository;
        }

        public string Prompt => "Press Enter if you're sure :)";

        public string Name => "ListRewards";

        public List<RewardGroup>? Execute(string input)
        {
            return rewardRepository.GetRewardGroups().ToList();
        }

        public string FormatOutput(List<RewardGroup> output)
        {
            var result = "";

            return output.Aggregate(result, (outputString, currentGroup) =>
            {
                Console.WriteLine(result);
                outputString += $"Group: {currentGroup.Name}";
                outputString += $"Score for completion: {currentGroup.ScoreForGroupCompletion}";
                outputString += Environment.NewLine;
                outputString += $"Rewards:";
                outputString += Environment.NewLine;

                outputString += currentGroup.Rewards.Aggregate("", (rewardsString, currentReward) =>
                {
                    rewardsString += "----";
                    rewardsString += $"Name: {currentReward.Name}, ScoreDifferential: {currentReward.ScoreDifferential}, ScoreRequired: {currentReward.ScoreRequired}";
                    rewardsString += Environment.NewLine;

                    return rewardsString;
                });
                return outputString;
            });
        }

        public string ParseUserInput(string input)
        {
            return input;
        }
    }
}

