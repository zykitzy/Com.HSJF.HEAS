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
    ///     Returns the result of multiplying the specified  value by negative one.
    /// </summary>
    /// <param name="d">The value to negate.</param>
    /// <returns>A decimal number with the value of , but the opposite sign.-or- Zero, if  is zero.</returns>
    public static Decimal Negate(this Decimal d)
    {
        return Decimal.Negate(d);
    }
}

}