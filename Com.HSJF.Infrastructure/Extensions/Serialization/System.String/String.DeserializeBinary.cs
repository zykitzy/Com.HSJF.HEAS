namespace Com.HSJF.Infrastructure.Extensions
{

// Copyright (c) 2015 ZZZ Projects. All rights reserved
// Licensed under MIT License (MIT) (https://github.com/zzzprojects/Z.ExtensionMethods)
// Website: http://www.zzzprojects.com/
// Feedback / Feature Requests / Issues : http://zzzprojects.uservoice.com/forums/283927
// All ZZZ Projects products: Entity Framework Extensions / Bulk Operations / Extension Methods /Icon Library

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static partial class Extensions
{
    /// <summary>
    ///     A string extension method that deserialize a string binary as &lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
    public static T DeserializeBinary<T>(this string @this)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T) binaryRead.Deserialize(stream);
        }
    }

    /// <summary>
    ///     A string extension method that deserialize a string binary as &lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
    public static T DeserializeBinary<T>(this string @this, Encoding encoding)
    {
        using (var stream = new MemoryStream(encoding.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T) binaryRead.Deserialize(stream);
        }
    }
}

}