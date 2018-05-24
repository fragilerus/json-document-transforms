// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Attributes
{
    /// <summary>
    /// Valid JDT attributes
    /// </summary>
    public interface IJdtAttribute
    {
        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        string Name { get; }
    }
}