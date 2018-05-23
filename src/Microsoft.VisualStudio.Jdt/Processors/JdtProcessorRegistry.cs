// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The central registry to manage supported processors
    /// </summary>
    public static class JdtProcessorRegistry
    {
        private static readonly IReadOnlyList<JdtProcessor> DefaultProcessors = new List<JdtProcessor>
        {
            new JdtRecurse(),
            new JdtRemove(),
            new JdtReplace(),
            new JdtRename(),
            new JdtMerge(),
            new JdtDefault()
        };

        private static readonly object Lock = new object();

        private static IList<JdtProcessor> processors = DefaultProcessors.ToList();

        /// <summary>
        /// The event that gets raised when the collection of processors gets changed
        /// </summary>
        internal static event Action JdtProcessorRegistryChanged = () => { };

        /// <summary>
        /// Gets a list of supported Processors, in the order of execution
        /// </summary>
        /// <returns>
        /// The ordered list of supported processors
        /// </returns>
        public static List<JdtProcessor> GetProcessors()
        {
            lock (Lock)
            {
                return new List<JdtProcessor>(processors);
            }
        }

        /// <summary>
        /// Insert a processor
        /// </summary>
        /// <param name="index">The zero-based index at which the processor should be inserted</param>
        /// <param name="processor">The processor to insert</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="processor"/> is null</exception>
        public static void Insert(int index, JdtProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor), $"{nameof(processor)} can not be null");
            }

            lock (Lock)
            {
                processors.Insert(index, processor);
                JdtProcessorRegistryChanged();
            }
        }

        /// <summary>
        /// Resets processors to the default state
        /// </summary>
        public static void Reset()
        {
            lock (Lock)
            {
                processors = DefaultProcessors.ToList();
                JdtProcessorRegistryChanged();
            }
        }
    }
}