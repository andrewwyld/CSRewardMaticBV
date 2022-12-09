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


## Questions
* Should the application be runnable?
* If yes, what should be the format - web/console/desktop/other application?
* Am I allowed to change pre-coded types (such as changing the `Score` property from `int` to `uint`)
* Is it ok if I include localisation in the final solution even if the bonus task states that no coding is required?
