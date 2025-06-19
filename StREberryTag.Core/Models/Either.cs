namespace StREberryTag.Core.Models;

public abstract class Either<TLeft, TRight>
{
    public readonly TLeft Left;
    public readonly TRight Right;

    protected Either(TLeft left)
    {
        Left = left;
    }

    protected Either(TRight right)
    {
        Right = right;
    }
}

public class Left<TLeft, TRight>(TLeft left) : Either<TLeft, TRight>(left);
public class Right<TLeft, TRight>(TRight right) : Either<TLeft, TRight>(right);
