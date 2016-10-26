namespace Com.HSJF.Infrastructure.Extensions
{

// Copyright (c) 2015 ZZZ Projects. All rights reserved
// Licensed under MIT License (MIT) (https://github.com/zzzprojects/Z.ExtensionMethods)
// Website: http://www.zzzprojects.com/
// Feedback / Feature Requests / Issues : http://zzzprojects.uservoice.com/forums/283927
// All ZZZ Projects products: Entity Framework Extensions / Bulk Operations / Extension Methods /Icon Library

using System;
using System.Linq;
using System.Text.RegularExpressions;

public static partial class Extensions
{
    /// <summary>
    ///     A string extension method that extracts all Decimal from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>All extracted Decimal.</returns>
    public static decimal[] ExtractManyDecimal(this string @this)
    {
        return Regex.Matches(@this, @"[-]?\d+(\.\d+)?")
            .Cast<Match>()
            .Select(x => Convert.ToDecimal(x.Value))
            .ToArray();
    }
}

}