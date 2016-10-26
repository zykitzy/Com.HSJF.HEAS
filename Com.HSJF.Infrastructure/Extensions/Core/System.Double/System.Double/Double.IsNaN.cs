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
    ///     Returns a value that indicates whether the specified value is not a number ().
    /// </summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>true if  evaluates to ; otherwise, false.</returns>
    public static Boolean IsNaN(this Double d)
    {
        return Double.IsNaN(d);
    }
}

}