// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Tests
{
    using System;
    using Newtonsoft.Json.Linq;
    using Processors;
    using Xunit;

    /// <summary>
    /// Test class for <see cref="JdtProcessorRegistry"/>
    /// </summary>
    public class JdtProcessorRegistryTest : IDisposable
    {
        private static readonly string SimpleSourceString = @"{ 'A': 1 }";

        private readonly JsonTransformationTestLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JdtProcessorRegistryTest"/> class.
        /// </summary>
        public JdtProcessorRegistryTest()
        {
            // xUnit creates a new instance of the class for each test, so a new logger is created
            this.logger = new JsonTransformationTestLogger();
        }

        /// <summary>
        /// Tests that processors can be added at a specific locations
        /// </summary>
        [Fact]
        public void AddProcessors()
        {
            var dummyProcessor = new DummyProcessor("stuff", () => { });
            JdtProcessorRegistry.Insert(1, dummyProcessor);
            Assert.Same(dummyProcessor, JdtProcessorRegistry.GetProcessors()[1]);
        }

        /// <summary>
        /// Tests that additional transform processors can be added
        /// </summary>
        [Fact]
        public void AdditionalVerbsShouldCallProcessor()
        {
            var wasCalled = false;
            var processor = new DummyProcessor("addme", () => wasCalled = true);
            string transformString = @"{
                                         '@jdt.addme': 'stuff' 
                                       }";
            JdtProcessorRegistry.Insert(0, processor);
            using (var transformStream = TestUtilities.GetStreamFromString(transformString))
            using (var sourceStream = TestUtilities.GetStreamFromString(SimpleSourceString))
            {
                JsonTransformation transform = new JsonTransformation(transformStream, this.logger);
                transform.Apply(sourceStream);
                Assert.True(wasCalled);
            }
        }

        /// <summary>
        /// Test cleanup
        /// </summary>
        public void Dispose()
        {
            JdtProcessorRegistry.Reset();
        }

        private class DummyProcessor : JdtProcessor
        {
            private readonly Action callback;

            public DummyProcessor(string verb, Action callback)
            {
                this.callback = callback;
                this.Verb = verb;
            }

            public override string Verb { get; }

            public override void Process(JObject source, JObject transform, JsonTransformationContextLogger logger)
            {
                this.callback();
                this.Successor.Process(source, transform, logger);
            }
        }
    }
}