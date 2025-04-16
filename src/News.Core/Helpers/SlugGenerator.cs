using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace News.Core.Helpers;

public static class SlugGenerator
{
    // white space, em-dash, en-dash, underscore
    static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

    // characters that are not valid
    static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

    // multiple hyphens
    static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

    public static string GenerateSlug(string value)
    {
        // convert to lower case
        value = value.ToLowerInvariant();

        // remove diacritics (accents)
        value = RemoveDiacritics(value);

        // ensure all word delimiters are hyphens
        value = WordDelimiters.Replace(value, "-");

        // strip out invalid characters
        value = InvalidChars.Replace(value, "");

        // replace multiple hyphens (-) with a single hyphen
        value = MultipleHyphens.Replace(value, "-");

        // trim hyphens (-) from ends
        return value.Trim('-');
    }

    /// See: http://www.siao2.com/2007/05/14/2629747.aspx
    private static string RemoveDiacritics(string stIn)
    {
        string stFormD = stIn.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (var t in stFormD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(t);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}