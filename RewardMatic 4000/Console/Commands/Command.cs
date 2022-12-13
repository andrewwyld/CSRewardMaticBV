using System;
namespace ConsoleApp.Commands
{
	public interface ICommand<TInput, TOutput>
	{
		string Prompt { get; }

		string Name { get; }

		TInput ParseUserInput(string input);

		TOutput? Execute(TInput input);

		string FormatOutput(TOutput output);
	}
}

