namespace Com.HSJF.Infrastructure.Extensions
{

// Copyright (c) 2015 ZZZ Projects. All rights reserved
// Licensed under MIT License (MIT) (https://github.com/zzzprojects/Z.ExtensionMethods)
// Website: http://www.zzzprojects.com/
// Feedback / Feature Requests / Issues : http://zzzprojects.uservoice.com/forums/283927
// All ZZZ Projects products: Entity Framework Extensions / Bulk Operations / Extension Methods /Icon Library

using System;

public static partial class Extensions
{
    /// <summary>
    ///     Returns the larger of two 8-bit signed integers.
    /// </summary>
    /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
    /// <returns>Parameter  or , whichever is larger.</returns>
    public static SByte Max(this SByte val1, SByte val2)
    {
        return Math.Max(val1, val2);
    }
}

}