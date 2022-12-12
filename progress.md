- [x] Ensure only positive input for `UpdateScore` as the score can’t be decremented
- [ ] Think about localisation
Currently, the only strings are the names of the rewards. These can be replaced by rewardIds, and later pass these to a localisation provider
What happens with the json input? We’d need to change the strings there to IDs too.

- [ ] Think about including a coefficient of the user’s score and reflect that in claimed awards
If a user’s score increases, he should receive the next awards
If a user’s score decreases, he should lose the awards up to the current number of points he has

- [ ] Think about concurrency
- Of the rewards repository
- Of the user class (`UpdateScore`)


# Questions
* Should the application be runnable?
  * Assumption - yes
* If yes, what should be the format - web/console/desktop/other application?
  * Assumption - console application sounds ok?
* Am I allowed to change pre-coded types (such as changing the `Score` property from `int` to `uint`)
  * Assumption - yes
* Is it ok if I include localisation in the final solution even if the bonus task states that no coding is required?
  * Assumption - yes
* What happens when we have two rewards with the same `ScoreDifferential`?
	* Assumption - according to the specification,
```
You can't spend the same points on more than one reward!
```
Which, as I understand it, means that at any given time we should have a single award, we’re trying to claim.
* How do we differentiate between rewards with the same `ScoreDifferential`?
	* Assumption - if we’re talking about reward groups, we should consider the reward from the “latest” reward group i.e. if groups 1 and 3 have rewards with score 300 and we have a score of 301, we should consider group 3 as the latest claimed award
* Is the code going to be run in a multi-threaded environment?
	* Assumption - yes
* What do you look for in a solution? There are several possible implementations that would favor different kind of efficiencies (memory, cpu) and I'm wondering about how the tradeoff should look like in this case.
* Are the query methods going to be called often? If yes, we'd need not only to search fast, but also to cache the responses
  * Assumption - yes




- [x] Add tests for what happens with unordered reward input

- [x] The current UserRewardUnitTests should actually be testing the repository class.

- [x] The Repository should also make accessible all available rewards, not just the service methods based on score.


## Reward Groups

The current structure:
```json
{
	"name": string,
	"rewards":
	[
		{"scoreDifferential": number, "name": string}
		...
	]
}
```

> It should be possible to get the RewardGroup for the Reward in progress

How we can do this without changing the structure: Search through all of the groups to find the reward in progress, then return the group


> It should be possible to get the RewardGroup for the latest Reward received

How we can do this without changing the structure: Search through all of the groups to find the latest reward received, then return the group

> The third thing we require is a way to get the latest RewardGroup the user has *entirely completed*. 
How we can do this without changing the structure: Find the last group, which has a last reward score less than the given score


We already know how to get the Latest Reward Received (LRR) and the Reward In Progress (RIP) from a list of rewards - use these and get their group name!

Attach a group name to the `Reward` objects.
Keep a dictionary of group names -> group instances for convenience.

We know if we have completed a group by knowing the group’s most “expensive” task.


What about ranges???

Rationale: I’d like the search to be O(1) (its best case scenario is log(n) now, if we employ something like binary search), even if we spend much time and reasonable memory on creation.

We can make a structure, that holds a set of ranges i.e.

[1,10] - would hold the reward at 1 (with the reward group), the reward at 10 (with the reward group) and optionally a completion of a reward group


We can insert all rewards in a dictionary and the key would be their value.

What about the holes?

We can insert dummy entries for all holes, but this would mean a lot of memory.


Can the repository work without having to separate between whether it's created by a plain rewards list or a rewards group list?

Maybe a group repository that holds groups info + holds a repository of plain rewards?

Rewards and groups

# TODO

- [ ] Actual console interface with a repl
- [ ] Make searching the rewards logarithmic
- [ ] Implement caching
- [ ] Think about handling localization runtime
- [ ] Think about handling user score adjustments