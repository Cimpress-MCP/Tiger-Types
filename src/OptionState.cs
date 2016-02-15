namespace Tiger.Types
{
    enum OptionState
        : byte // todo(cosborn) Does this save anything?  How does it pack?
    {
        None, // note(cosborn) None must be the 0 value in case of default(Option<T>).
        Some
    }
}