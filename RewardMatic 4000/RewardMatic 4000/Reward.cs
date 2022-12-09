namespace RewardMatic_4000
{
    public class Reward
    {
        public static Reward[] AvailableRewards = new Reward[]
        {
            new Reward(200, "Starting strong!"),
            new Reward(300, "Getting better!"),
            new Reward(400, "Nice job!"),
            new Reward(500, "Keep going for cake!"),
            new Reward(600, "OK, there's no cake. Keep going anyway!"),
            new Reward(700, "YOU FINISHED! You are the only person to do this."),
        };

        // the score you need to attain to get the reward

        // the reward message

        private Reward(int scoreDifferential, string message)
        {
            ScoreDifferential = scoreDifferential;
            Message = message;
        }

        public int ScoreDifferential { get; }

        public string Message { get; }
    }
}