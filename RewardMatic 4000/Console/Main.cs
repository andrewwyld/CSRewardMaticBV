#nullable enable

using Newtonsoft.Json;
using RewardMatic_4000;

namespace ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var groups = ParseRewardGroups("rewards.json");

        var rewardRepository = RewardRepository.FromJson("rewards.json");
        var rewardService = new RewardService(rewardRepository);

        if (groups is null)
        {
            Console.WriteLine("null groups");
            return;
        }

        groups.ForEach(group =>
        {
            Console.WriteLine(group.Name);
        });

    }

    public static List<RewardGroup>? ParseRewardGroups(string filename)
    {
        using (var reader = new StreamReader(filename))
        {
            var content = reader.ReadToEnd();

            if (content is null)
            {
                return null;
            }

            var groups = JsonConvert.DeserializeObject<List<RewardGroup>>(content);
            return groups;
        }
    }
}

