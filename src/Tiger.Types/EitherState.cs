namespace Tiger.Types
{
    /// <summary>Represents the allowable states of an <see cref="Either{TLeft,TRight}"/>.</summary>
    enum EitherState
        : byte // todo(cosborn) Does this save anything?  How does it pack?
    {
        /// <summary>An uninitialized state; represents an error condition.</summary>
        Bottom, // note(cosborn) Must be 0 in the case of default(Either<TLeft, TRight>).

        /// <summary>The Left state; usually represents a bad case.</summary>
        Left,

        /// <summary>The Right state; usually represents a good case.</summary>
        Right
    }
}
