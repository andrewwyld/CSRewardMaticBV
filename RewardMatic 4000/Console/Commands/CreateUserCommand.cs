using System;
using RewardMatic_4000;

namespace ConsoleApp.Commands
{
	public class CreateUserInput
    {
        public string Name { get; set; }
    }

    public class CreateUserCommand : ICommand<CreateUserInput, User?>
    {
        private readonly IUserRepository userRepository;

        public CreateUserCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string Name => "CreateUser";

        public string Prompt => "How would you like your new user to be called?";

        public CreateUserInput ParseUserInput(string input)
        {
            return new CreateUserInput
            {
                Name = input
            };
        }

        public User? Execute(CreateUserInput input)
        {
            return userRepository.CreateUser(input.Name);
        }

        public string FormatOutput(User? output)
        {
            if (output is null)
            {
                return "No user created :(";
            }
            return $"User with name: {output?.Name} created!";
        }
    }
}

