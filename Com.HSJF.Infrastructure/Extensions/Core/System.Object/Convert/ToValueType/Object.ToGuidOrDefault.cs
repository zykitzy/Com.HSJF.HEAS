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
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Guid defaultValue)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a unique identifier or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a GUID.</returns>
    public static Guid ToGuidOrDefault(this object @this, Func<Guid> defaultValueFactory)
    {
        try
        {
            return new Guid(@this.ToString());
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
}

}