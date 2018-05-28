// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Processors
{
    using System;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a transformation
    /// </summary>
    public abstract partial class JdtProcessor
    {
        /// <summary>The instance of the processor chain to be used</summary>
        /// <remarks>
        /// Needs to be lazy due to circular dependency issue with the <see cref="JdtProcessorRegistry"/>
        /// </remarks>
        private static readonly Lazy<JdtProcessorChain> ProcessorChain = new Lazy<JdtProcessorChain>();
        private JdtProcessor successor;

        /// <summary>
        /// Gets the JDT verb corresponding to this transformation.
        /// Can be null or empty.
        /// Does not include the preffix (@jdt.)
        /// </summary>
        public abstract string Verb { get; }

        /// <summary>
        /// Gets the full verb corresponding the to the transformation
        /// </summary>
        protected string FullVerb => this.Verb == null ? null : JdtUtilities.JdtSyntaxPrefix + this.Verb;

        /// <summary>
        /// Gets the successor of the current transformation
        /// </summary>
        protected JdtProcessor Successor
        {
            // Defaults to the end of chain processor
            get => this.successor ?? JdtEndOfChain.Instance;
            private set => this.successor = value;
        }

        /// <summary>
        /// Executes the transformation
        /// </summary>
        /// <param name="source">Object to be transformed</param>
        /// <param name="transform">Object specifying the transformation</param>
        /// <param name="logger">The logger for the transformation</param>
        public abstract void Process(JObject source, JObject transform, JsonTransformationContextLogger logger);

        /// <summary>
        /// Executes the entire transformation with the given objects
        /// Mutates the source object
        /// </summary>
        /// <param name="source">Object to be transformed</param>
        /// <param name="transform">Object that specifies the transformation</param>
        /// <param name="logger">The logger for the transformation</param>
        internal static void ProcessTransform(JObject source, JObject transform, JsonTransformationContextLogger logger)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            // Passes in a clone of the transform object because it can be altered during the transformation process
            ProcessorChain.Value.Start(source, (JObject)transform.CloneWithLineInfo(), logger);
        }
    }
}
