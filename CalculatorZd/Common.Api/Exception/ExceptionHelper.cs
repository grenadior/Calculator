using System;
using System.Diagnostics;

namespace Common.Api.Exceptions
{
    public static class ExceptionHelper
    {
        [Conditional("DEBUG")]
        public static void ThrowIfDebug(Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }
}
