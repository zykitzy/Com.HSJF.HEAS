namespace Com.HSJF.Infrastructure.Extensions
{

// Copyright (c) 2015 ZZZ Projects. All rights reserved
// Licensed under MIT License (MIT) (https://github.com/zzzprojects/Z.ExtensionMethods)
// Website: http://www.zzzprojects.com/
// Feedback / Feature Requests / Issues : http://zzzprojects.uservoice.com/forums/283927
// All ZZZ Projects products: Entity Framework Extensions / Bulk Operations / Extension Methods /Icon Library

using System;
using System.Text;

public static partial class Extensions
{
    /// <summary>
    ///     A string extension method that extracts the UInt32 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted UInt32.</returns>
    public static uint ExtractUInt32(this string @this)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < @this.Length; i++)
        {
            if (Char.IsDigit(@this[i]))
            {
                sb.Append(@this[i]);
            }
        }

        return Convert.ToUInt32(sb.ToString());
    }
}

}