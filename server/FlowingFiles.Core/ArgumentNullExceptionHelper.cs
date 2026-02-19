namespace FlowingFiles.Core;

public static class ArgumentNullExceptionHelper
{
    public static void ThrowIfNull(object? argument, string paramName)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }
}
