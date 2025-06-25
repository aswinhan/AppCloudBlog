namespace AppCloudBlog.Application.Common.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        string str = phrase.ToLowerInvariant().Trim();

        // Remove invalid characters
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // Convert multiple spaces into one hyphen
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = Regex.Replace(str, @"\s", "-"); // Replace space with -

        // Remove multiple consecutive dashes
        str = Regex.Replace(str, @"-+", "-");

        return str;
    }
}
