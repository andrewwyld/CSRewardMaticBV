# RewardMatic 4000

## Run console app with Docker

1. Install docker [from here](https://docs.docker.com/desktop/install/mac-install/)
2. Go to the solution folder and run `docker build -t rewards-console .`
3. After the build is done, run `docker run -i rewards-console`
4. Interact with the system

## Structure

### RewardMatic_4000  
A library that holds business logic about users, rewards and reward groups.

It is based mostly on repositories and services. Repositories are only responsible for storing the entities and the business logic after retrieval happens in the services.

The user class does not hold any internal state other than its score and the service methods only require a score.

This way, the service can be extended with a cache and both the service and the user classes will be practically stateless.



### RewardMaticTests  
Unit Tests for the RewardMatic_4000 library. Introduced `Moq` in order to stub methods


### ConsoleApp  
REPL application for interacting with users and rewards.

The happy path goes something like this:
1. `ViewAvailableCommands`
2. `LoadFile` - load a json file with rewards (`rewards.json` is available)
3. `ViewRewards` - verify that the LoadFile has done a good job
4. `CreateUser` - to start interacting with an actual user
5. `ViewUserRewards` - to see that initially, the user has no rewards
6. `IncrementUserScore` - buff the guy
7. Repeat `5.` and `6.`
8. `Exit`



# Bonus tasks

> Describe how you would localise this code to use multiple languages, while keeping the logic of the existing system intact.

The only user-facing strings in the program are the reward and reward group names. I'd replace these strings to hold not the names of the entities, but rather identifiers (again strings). These identifiers can be put in a table where each row will have this identifier and then several columns with the entity name in different languages.

This kind of data structure (let's imagine an SQL table for simplicity) can then be used by a localization provider that will be created by specifying a language. Then, whenever we want to display a reward name or a reward group name, we first go to the localization provider and retrieve the correct name for our current language and the identifier we have.


> Please also think about what might happen if we wish to apply a scale factor to a user's score, to reflect their ability, after they have already received some rewards. What is desirable behaviour for rewards the user has already received? How would you deal with this?

In general, several things can happen:
1. The user can lose his/her existing rewards that are no longer reachable
given the newly scaled points, or gain new rewards if the scale is above 1.

2. The other thing that I imagine is the user preserving the rewards if the scale is below 1, but then still have to climb the full ladder to the next rewards i.e.

The user has a score of 500 and rewards
* A - 100
* B - 200
* C - 200

The next rewards are:

* D - 500
* E - 200

If we apply a scale of 1/2 to the user score, he'd have 250, which is
just above A.
In the described scenario, he still has B and C awards, but would need
750 more points to reach D (and not 500, as if he has B and C).

It really depends on why we want to scale the user's score. It would be motivating for the users to see their progress in the game preserved and acknowledged (this leans towards option 2.).

On the other hand there might be cases of cheating and so after scaling down the user points it would be fair to remove their rewards as well (has to be proven though :D)

Currently, the system would make the rewards disappear if the user's score goes down, as the methods are just based on score and nothing's saved in the user class itself.
