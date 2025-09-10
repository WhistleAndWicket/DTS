using NJsonSchema;
using NSwag;
using NSwag.Generation;
using NSwag.Generation.AspNetCore;

namespace Api
{
    /// <summary>
    /// A wrapper class for generating OpenAPI documents using <see cref="AspNetCoreOpenApiDocumentGenerator"/>.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IOpenApiDocumentGenerator"/> interface and utilises the
    /// <see cref="AspNetCoreOpenApiDocumentGenerator"/> to generate OpenAPI documents based on the provided
    /// <paramref name="serviceProvider"/>.
    /// </remarks>
    /// <param name="serviceProvider">The service provider used to resolve services.</param>
    public class GeneratorWrapper(IServiceProvider serviceProvider) : IOpenApiDocumentGenerator
    {
        /// <summary>
        /// Converts the property names of all schemas in the OpenAPI document to camelCase.
        /// </summary>
        /// <param name="document">The OpenAPI document whose schema property names will be converted.</param>
        private static void ApplyCamelCaseNaming(OpenApiDocument document)
        {
            foreach (var schema in document.Definitions.Values)
            {
                if (schema.Properties != null)
                {
                    var camelCaseProperties = new Dictionary<string, JsonSchemaProperty>();

                    foreach (var prop in schema.Properties)
                    {
                        // Convert the property name to camelCase.
                        var camelCaseKey = char.ToLowerInvariant(prop.Key[0]) + prop.Key.Substring(1);
                        camelCaseProperties[camelCaseKey] = prop.Value;
                    }

                    // Update schema properties by creating a new dictionary.
                    schema.Properties.Clear();
                    foreach (var kvp in camelCaseProperties)
                    {
                        schema.Properties.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<OpenApiDocument> GenerateAsync(string documentName)
        {
            var generator = new AspNetCoreOpenApiDocumentGenerator(
                new AspNetCoreOpenApiDocumentGeneratorSettings());
            var document = await generator.GenerateAsync(serviceProvider);

            // Post-process to ensure camelCase property names.
            ApplyCamelCaseNaming(document);

            return document;
        }
    }
}