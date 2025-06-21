using System.CommandLine;
using System.CommandLine.Binding;

namespace reberry;

public class CommandBuilder(Command root)
{
    Command command = root;

    public CommandBuilder WithArgument<T>(Argument<T> arg)
    {
        command.AddArgument(arg);

        return this;
    }

    public CommandBuilder WithOption<T>(Option<T> option)
    {
        command.AddOption(option);

        return this;
    }

    public CommandBuilder WithSubCommand(Command sub)
    {
        command.AddCommand(sub);

        return this;
    }
    
    public CommandBuilder WithAsyncHandler<T>(Func<T, Task> handler, BinderBase<T> binder)
    {
        command.SetHandler(handler, binder);

        return this;
    }

    public Command Build() => 
        command;
}
