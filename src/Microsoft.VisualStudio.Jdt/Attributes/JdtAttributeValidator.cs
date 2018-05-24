// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Jdt.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validator for JDT attributes within a verb object
    /// </summary>
    public class JdtAttributeValidator
    {
        private readonly IReadOnlyDictionary<string, IJdtAttribute> validAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="JdtAttributeValidator"/> class.
        /// </summary>
        /// <param name="validAttribute">The attributes that are valid</param>
        public JdtAttributeValidator(params IJdtAttribute[] validAttribute)
        {
            this.validAttributes = validAttribute?.ToDictionary(d => d.FullName()) ?? new Dictionary<string, IJdtAttribute>();
        }

        /// <summary>
        /// Validates the object and returns the appropriate attributes contained within it.
        /// </summary>
        /// <param name="transformObject">The object to validade</param>
        /// <returns>A dictionary with the JToken attributes of each valid attribute.
        /// An empty dictionary is returned if no valid properties are found</returns>
        /// <exception cref="JdtException">Throws if an invalid jdt attribute is found.</exception>
        public Dictionary<IJdtAttribute, JToken> ValidateAndReturnAttributes(JObject transformObject)
        {
            if (transformObject == null)
            {
                throw new ArgumentNullException(nameof(transformObject));
            }

            Dictionary<IJdtAttribute, JToken> attributes = new Dictionary<IJdtAttribute, JToken>();

            // First, we look through all of the properties that have JDT syntax
            foreach (JProperty property in transformObject.GetJdtProperties())
            {
                if (!this.validAttributes.TryGetValue(property.Name, out var attribute))
                {
                    // TO DO: Specify the transformation in the error
                    // If the attribute is not supported in this transformation, throw
                    throw JdtException.FromLineInfo(string.Format(Resources.ErrorMessage_InvalidAttribute, property.Name), ErrorLocation.Transform, property);
                }
                else
                {
                    // If it is a valid JDT attribute, add its value to the dictionary
                    attributes.Add(attribute, property.Value);
                }
            }

            // If the object has attributes, it should not have any other properties in it
            if (attributes.Count > 0 && attributes.Count != transformObject.Properties().Count())
            {
                throw JdtException.FromLineInfo(Resources.ErrorMessage_InvalidAttributes, ErrorLocation.Transform, transformObject);
            }

            return attributes;
        }
    }
}
