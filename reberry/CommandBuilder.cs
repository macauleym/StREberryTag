using System.CommandLine;
using System.CommandLine.Binding;

namespace reberry;

public class CommandBuilder(RootCommand root)
{
    RootCommand rootCommand = root;

    public CommandBuilder WithArgument<T>(Argument<T> arg)
    {
        rootCommand.AddArgument(arg);

        return this;
    }

    public CommandBuilder WithOption<T>(Option<T> option)
    {
        rootCommand.AddOption(option);

        return this;
    }

    public CommandBuilder WithAsyncHandler<T>(Func<T, Task> handler, BinderBase<T> binder)
    {
        rootCommand.SetHandler(handler, binder);

        return this;
    }

    public RootCommand Build() => 
        rootCommand;
}
