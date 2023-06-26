
using System.Text;

internal class GeneratorUtils
{
    internal static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
        "UTF-8",
        new EncoderReplacementFallback(string.Empty),
        new DecoderExceptionFallback()
    );

}