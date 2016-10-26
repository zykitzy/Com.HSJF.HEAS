namespace Com.HSJF.Infrastructure.Extensions
{

using System;
using System.Text;

public static partial class Extensions
{
    /// <summary>
    ///     A StringBuilder extension method that extracts the string single quote described by
    ///     @this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted string single quote.</returns>
    public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this)
    {
        return @this.ExtractStringSingleQuote(0);
    }
    /// <summary>A StringBuilder extension method that extracts the string single quote described by
    /// @this.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="endIndex">[out] The end index.</param>
    /// <returns>The extracted string single quote.</returns>
    public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, out int endIndex)
    {
        return @this.ExtractStringSingleQuote(0, out endIndex);
    }


    /// <summary>
    ///     A StringBuilder extension method that extracts the string single quote described by
    ///     @this.
    /// </summary>
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <param name="this">The @this to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <returns>The extracted string single quote.</returns>
    public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, int startIndex)
    {
        int endIndex;
        return @this.ExtractStringSingleQuote(startIndex, out endIndex);
    }
    /// <summary>A StringBuilder extension method that extracts the string single quote described by
    /// @this.</summary>
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <param name="this">The @this to act on.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="endIndex">[out] The end index.</param>
    /// <returns>The extracted string single quote.</returns>
    public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, int startIndex, out int endIndex)
    {
        var sb = new StringBuilder();

        if (@this.Length > startIndex + 1)
        {
            var ch1 = @this[startIndex];

            if (ch1 == '\'')
            {
                // WARNING: This is not a valid string, however single quote is often used to make it more readable in text templating
                // 'my string'

                var pos = startIndex + 1;

                while (pos < @this.Length)
                {
                    var ch = @this[pos];
                    pos++;

                    char nextChar;
                    if (ch == '\\' && pos < @this.Length && ((nextChar = @this[pos]) == '\\' || nextChar == '\''))
                    {
                        sb.Append(nextChar);
                        pos++; // Treat as escape character for \\ or \"
                    }
                    else if (ch == '\'')
                    {
                        endIndex = pos;
                        return sb;
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }

                throw new Exception("Unclosed string starting at position: " + startIndex);
            }
        }

        endIndex = -1;
        return null;
    }
}

}