namespace Apparat.Helpers
{
    public static partial class ValidationParseHelper
    {
        public static string RemovingSpacesFromText(string oldHostname)
        {
            if (oldHostname is null) return oldHostname;

            string newHostname = oldHostname.Replace(" ", "");
            return newHostname;
        }
    }
}
