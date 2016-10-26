namespace Com.HSJF.Infrastructure.Extensions
{

// Copyright (c) 2015 ZZZ Projects. All rights reserved
// Licensed under MIT License (MIT) (https://github.com/zzzprojects/Z.ExtensionMethods)
// Website: http://www.zzzprojects.com/
// Feedback / Feature Requests / Issues : http://zzzprojects.uservoice.com/forums/283927
// All ZZZ Projects products: Entity Framework Extensions / Bulk Operations / Extension Methods /Icon Library

using System;
using System.IO;
using System.Text;

public static partial class Extensions
{
    /// <summary>
    ///     A string extension method that converts the @this to a MemoryStream.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a MemoryStream.</returns>
    public static Stream ToMemoryStream(this string @this)
    {
        Encoding encoding = Activator.CreateInstance<ASCIIEncoding>();
        return new MemoryStream(encoding.GetBytes(@this));
    }
}

}