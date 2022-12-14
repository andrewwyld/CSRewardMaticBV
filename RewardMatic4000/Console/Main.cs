#nullable enable

using ConsoleApp.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RewardMatic_4000;

namespace ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        RunRepl();
    }

    private static void RunRepl()
    {
        // Default values prior to assigning proper ones based on user input
        IRewardRepository rewardRepository = new RewardRepository(new List<Reward>());

        IRewardService rewardService = new RewardService(rewardRepository);

        IUserRepository userRepository = new UserRepository(rewardService);

        int availableCommandsIndex = 0;

        var userCreated = false;

        ViewAvailableCommands(availableCommandsIndex);

        string input = "";

        while (input != "Exit")
        {
            Console.WriteLine("Please type a command name below");
            input = Console.ReadLine();

            if (input == "ViewAvailableCommands")
            {
                ViewAvailableCommands(availableCommandsIndex);
            }
            else if (input == "LoadFile")
            {
                var command = new LoadFileCommand();
                var output = RunCommand<LoadFileCommand, LoadFileCommandInput, LoadFileCommandOutput>(new LoadFileCommand());

                if (output is null || output.RewardService is null)
                {
                    continue;
                }

                rewardRepository = output.RewardRepository;
                userRepository = new UserRepository(output.RewardService);
                availableCommandsIndex++;
                Console.WriteLine("New commands are available");
                ViewAvailableCommands(availableCommandsIndex);
            }
            else if (input == "ViewRewards")
            {
                if (availableCommandsIndex < 1)
                {
                    Console.WriteLine("You must first load a file to view the rewards");
                }
                var command = new ViewRewardsCommand(rewardRepository);
                RunCommand<ViewRewardsCommand, string, List<RewardGroup>>(command);
            }
            else if (input == "CreateUser")
            {
                if (availableCommandsIndex < 1)
                {
                    Console.WriteLine("You must first load a file with rewards to create a user");
                    continue;
                }

                var command = new CreateUserCommand(userRepository);
                RunCommand<CreateUserCommand, CreateUserInput, User?>(command);
                if (!userCreated)
                {
                    availableCommandsIndex++;
                    userCreated = true;
                    Console.WriteLine("New commands area available!");
                    ViewAvailableCommands(availableCommandsIndex);
                }
            }
            else if (input == "IncrementUserScore")
            {
                if (availableCommandsIndex < 2)
                {
                    Console.WriteLine("You must create a user in order to increment its score");
                    continue;
                }

                var command = new IncrementUserScore(userRepository);
                RunCommand<IncrementUserScore, IncrementUserScoreInput, User>(command);
            }
            else if (input == "ViewUserRewards")
            {
                if (availableCommandsIndex < 2)
                {
                    Console.WriteLine("You must create a user in order to view his rewards");
                    continue;
                }

                var command = new ViewUserRewardsCommand(userRepository);
                RunCommand<ViewUserRewardsCommand, ViewUserRewardsInput, ViewUserRewardsOutput>(command);
            }
            else
            {
                Console.WriteLine("Unrecognized command, please see the available commands");
                ViewAvailableCommands(availableCommandsIndex);
            }
        }
    }


    // Available commands through the different stages of the app
    private static List<List<string>> AvailableCommands = new List<List<string>>
    {
        new List<string> { "LoadFile", "Exit", "ViewAvailableCommands" },
        new List<string> { "ViewRewards", "CreateUser", "Exit", "ViewAvailableCommands" },
        new List<string> { "ViewRewards", "CreateUser", "IncrementUserScore", "ViewUserRewards", "Exit", "ViewAvailableCommands"}
    };

    private static void ViewAvailableCommands(int index)
    {
        Console.WriteLine("Available commands: ");
        Console.WriteLine(string.Join(", ", AvailableCommands[index]));
    }

    private static TCommandOutput? RunCommand<TCommand, TCommandInput, TCommandOutput>(TCommand command)
        where TCommand: ICommand<TCommandInput, TCommandOutput>
    {
        Console.WriteLine(command.Prompt);

        var input = Console.ReadLine();

        TCommandInput commandInput;

        try
        {
            commandInput = command.ParseUserInput(input);
        }
        catch (Exception e)
        {
            Console.WriteLine($"The command failed to parse the input: {e.Message}");
            return default(TCommandOutput);
        }

        TCommandOutput output;

        try
        {
            output = command.Execute(commandInput);
            Console.WriteLine(command.FormatOutput(output));
        }
        catch (Exception e)
        {
            Console.WriteLine($"The command failed to execute: {e.Message}");
            return default(TCommandOutput);
        }
        return output;
    }
}

