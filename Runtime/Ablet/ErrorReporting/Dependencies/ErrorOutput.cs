namespace Ablet.ErrorReporting.Dependencies
{
    static class ErrorOutput
    {
        public static IErrorOutput Instance = new DefaultErrorOutput();
    }
}
