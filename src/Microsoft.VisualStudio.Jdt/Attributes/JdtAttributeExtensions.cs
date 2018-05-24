// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Attributes
{
    /// <summary>
    /// Implements extensions for <see cref="IJdtAttribute"/>
    /// </summary>
    internal static class JdtAttributeExtensions
    {
        /// <summary>
        /// Get the full name of an attribute, with the JDT prefix
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <returns>A string with the full name of the requested attribute</returns>
        internal static string FullName(this IJdtAttribute attribute)
        {
            return JdtUtilities.JdtSyntaxPrefix + attribute.Name.ToLower();
        }
    }
}
