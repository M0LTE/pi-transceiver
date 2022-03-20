namespace eng_test
{
    internal class BuildInfo
    {
        public static string? GetVersion()
        {
            return typeof(Program)?.Assembly?.GetName()?.Version?.ToString();
        }
    }
}
