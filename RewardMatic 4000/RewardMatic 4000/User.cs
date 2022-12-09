#nullable enable
using System;
using System.Collections.Generic;

namespace RewardMatic_4000
{
    public class User
    {
        private int _score = 0;

        public User()
        {
        }

        public int Score
        {
            get { return _score; }
        }

        public void UpdateScore(int update)
        {
            _score += update;
        }

        public Reward? GetRewardInProgress()
        {
            throw new NotImplementedException();
        }

        public Reward? GetLatestRewardReceived()
        {
            throw new NotImplementedException();
        }
    }
}