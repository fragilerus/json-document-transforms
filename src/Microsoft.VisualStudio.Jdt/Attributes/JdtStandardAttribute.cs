// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Attributes
{
    /// <summary>
    /// Holds the collection of standard JDT attributes
    /// </summary>
    public class JdtStandardAttribute : IJdtAttribute
    {
        /// <summary>
        /// The JDT path attribute
        /// </summary>
        public static readonly IJdtAttribute Path = new JdtStandardAttribute("path");

        /// <summary>
        /// The JDT value attribute
        /// </summary>
        public static readonly IJdtAttribute Value = new JdtStandardAttribute("value");

        private JdtStandardAttribute(string name) => this.Name = name;

        /// <inheritdoc />
        public string Name { get; }
    }
}