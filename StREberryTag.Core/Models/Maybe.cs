namespace StREberryTag.Core.Models;

public abstract class Maybe<T>
{
    public readonly T Value;

    protected Maybe()
    {
        Value = default;
    }
    
    protected Maybe(T value)
    {
        Value = value;
    }
}

public class Just<T>(T value) : Maybe<T>(value);
public class None<T> : Maybe<T>;
