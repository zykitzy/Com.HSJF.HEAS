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
    ///     Returns the larger of two 16-bit unsigned integers.
    /// </summary>
    /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
    /// <returns>Parameter  or , whichever is larger.</returns>
    public static UInt16 Max(this UInt16 val1, UInt16 val2)
    {
        return Math.Max(val1, val2);
    }
}

}