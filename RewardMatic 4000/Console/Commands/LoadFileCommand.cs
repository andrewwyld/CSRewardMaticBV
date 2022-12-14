using System;
using System.Linq;
using RewardMatic_4000;

namespace ConsoleApp.Commands
{
	public class LoadFileCommandOutput
	{
		public IRewardService? RewardService { get; set; }

        public IRewardRepository? RewardRepository { get; set; }

		public int RewardGroupsCount { get; set; }
	}

    public class LoadFileCommandInput
    {
        public string FileName { get; set; } = "rewards.json";
    }

    public class LoadFileCommand : ICommand<LoadFileCommandInput, LoadFileCommandOutput>
    {
        public string Name => "LoadFile";

        public string Prompt => "Please enter the path of the json file you wish to load reward groups from";

        public LoadFileCommandInput ParseUserInput(string input)
        {
            return new LoadFileCommandInput
            {
                FileName = input
            };
        }

        public LoadFileCommandOutput? Execute(LoadFileCommandInput input)
        {
            var repository = RewardRepository.FromJson(input.FileName);

            if (repository is null)
            {
                throw new Exception("Could not read json information from the provided file");
            }

            var groupsCount = repository.GetRewardGroups().Count();

            return new LoadFileCommandOutput
            {
                RewardService = new RewardService(repository),
                RewardRepository = repository,
                RewardGroupsCount = groupsCount
            };
        }

        public string FormatOutput(LoadFileCommandOutput output)
        {
            return $"Created {output.RewardGroupsCount} reward Groups. Use ViewRewards to see them!";
        }
    }
}

