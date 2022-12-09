# The RewardMatic 4000
Welcome to the RewardMatic 4000 code test!

This is a partially-implemented system that rewards users for scoring a certain number of points in a game. In this test, you will complete the simple implementation defined by the test class, then write an extension of that system to have a larger number of rewards organized in sections (reward groups). To do this, you will load the data in **rewards.json** and use it to populate `Reward` and another class called `RewardGroup`, which the `User` class will make use of to give users a more complete set of rewards.

Feel free to change the architecture of this system as you think fit, but the behaviour should be as described below.

In this test you will:
- implement functions in the `User` class to pass tests in the `UserRewardUnitTest`
- write tests for, and implement, code to load rewards in groups from **rewards.json** and show relevant `Reward`s and `RewardGroup`s for a user with a given score.

We recommend you read the entire task description before starting. You are free to condense steps of this task together if you see fit; please also create as many helper classes as you like. We are looking for clear, sensible implementations with a good level of unit testing.

## Step 1: implement `User` as defined by `UserRewardUnitTest`

A `User` object is a toy version of a user. Each user has a score; their score can increase, but not decrease, and this is done through the `Update(int)` function. You can see the behaviour in `TestScoreIncrementsCorrectly()` in the `UserRewardUnitTest` class.

A `Reward` object is a treat the user gets when their score passes a threshold: it contains the score differential they need to achieve get the reward, and the message the user sees when they are rewarded.

`Reward` contains a static array of `Reward`s called `AvailableRewards`. You can use this array to complete the first part of the task!

A quick note on how to use the score differentials: `Reward.AvailableRewards[0]` has a `ScoreDifferential` of 200, so the user needs to get over 200 points to win this reward. `Reward.AvailableRewards[1]` has a `ScoreDifferential` of 300, so the user needs to get an *additional* 300 points to win this reward: that's a *total* of 500. You can think of the `ScoreDifferential` like the "price" of the reward, and as the user accumulates a greater score, they can "spend" their score to get new rewards. You can't spend the same points on more than one reward!

`User.GetRewardInProgress()` should always return the reward the user is working towards, unless they have achieved them all; then it should return `null`.

`User.GetLatestRewardReceived()` should always return the last reward the user received, unless they haven't received any; then it should return `null`.

The existing tests are designed to reflect this. You are welcome to add to these tests to include more detail if you wish, as long as they reflect the desired behaviour above.

## Step 2: extend the reward system to use reward groups

We now wish to extend the system to use data from **rewards.json**. We want to use this data directly. We might want to change the data, adding more rewards or changing the names of things, later. (We'll come back to this in the bonus task section!)

The extension should allow three new things. The first two are:
- it should be possible to get the `RewardGroup` for the `Reward` in progress
- it should be possible to get the `RewardGroup` for the latest `Reward` received

For instance, if a user has completed three of the first group of `Reward`s, then both of these would be the *first* `RewardGroup`. However, if they have completed all six `Reward`s in the first group, then the `RewardGroup` of the latest `Reward` received is the *first* `RewardGroup`, but the `RewardGroup` of the `Reward` in progress is the *second* `Reward` group.

You will write tests for these new functions in a similar way to the tests for the existing system. Exactly how you choose to implement this is up to you, but we want to know, from a single function call, what the answer is to either of those questions, as well as maintain the system's existing behaviour for `Reward`s.

The third thing we require is a way to get the latest `RewardGroup` the user has *entirely completed*. For example: if a user has completed three of the first group of rewards, then that user hasn't completed *any* `RewardGroup`s. If a user has completed all six rewards in the first group, or if a user has completed three of the second group of rewards, then that user has completed the *first* `RewardGroup`. When the user has completed all the `Reward`s, then they have completed the *last* `RewardGroup`.

## Bonus task

You are not required to code anything for this task. Imagine that the system you have completed has been deployed. Describe how you would localise this code to use multiple languages, while keeping the logic of the existing system intact.

Please also think about what might happen if we wish to apply a scale factor to a user's score, to reflect their ability, after they have already received some rewards. What is desirable behaviour for rewards the user has already received? How would you deal with this?

## Good luck!
