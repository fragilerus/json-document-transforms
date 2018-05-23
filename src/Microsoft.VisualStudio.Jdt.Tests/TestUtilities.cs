// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Tests
{
    using System.IO;

    /// <summary>
    /// Utility class for the test project
    /// </summary>
    internal class TestUtilities
    {
        /// <summary>
        /// Creates a memory stream from a string
        /// </summary>
        /// <param name="s">the string to covert to the stream</param>
        /// <returns>a stream representing the string <paramref name="s"/></returns>
        internal static Stream GetStreamFromString(string s)
        {
            MemoryStream stringStream = new MemoryStream();
            StreamWriter stringWriter = new StreamWriter(stringStream);
            stringWriter.Write(s);
            stringWriter.Flush();
            stringStream.Position = 0;

            return stringStream;
        }
    }
}