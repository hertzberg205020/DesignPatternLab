namespace CardGameFramework.Big2.Helpers;

public static class StringExtensions
{
  public static string Repeat(this string text, uint n)
  {
    if (text == null)
      throw new ArgumentNullException(nameof(text));

    if (text.Length == 0 || n == 0)
      return string.Empty;

    var textAsSpan = text.AsSpan();
    var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
    for (var i = 0; i < n; i++)
    {
      textAsSpan.CopyTo(span.Slice((int)i * textAsSpan.Length, textAsSpan.Length));
    }

    return new string(span);
  }
}
