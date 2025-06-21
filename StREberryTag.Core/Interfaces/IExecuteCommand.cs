namespace StREberryTag.Core.Interfaces;

public interface IExecuteCommand
{
    public Task<string> ItAsync(string command);
}
