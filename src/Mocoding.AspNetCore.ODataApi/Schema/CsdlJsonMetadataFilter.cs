using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using NJsonSchema;

namespace Mocoding.AspNetCore.ODataApi.Schema
{
    public class CsdlJsonMetadataFilter : IAsyncResultFilter
    {
        //public void OnResourceExecuting(ResourceExecutingContext context)
        //{
        //    // do nothing
        //}

        //public void OnResourceExecuted(ResourceExecutedContext context)
        //{
        //    if (context.HttpContext.Request.Path.Value.Contains("$metadata")
        //        && context.HttpContext.Request.Query.ContainsKey("$format")
        //        && context.HttpContext.Request.Query["$format"].Count > 0
        //        && context.HttpContext.Request.Query["$format"][0] == "json")
        //    {
        //        Console.WriteLine("Catched");
        //    }
        //}

        //public void OnResultExecuting(ResultExecutingContext context)
        //{
        //    if (context.HttpContext.Request.Path.Value.Contains("$metadata")
        //        && context.HttpContext.Request.Query.ContainsKey("$format")
        //        && context.HttpContext.Request.Query["$format"].Count > 0
        //        && context.HttpContext.Request.Query["$format"][0] == "json")
        //    {
        //        context.Cancel = true;
        //        if (context.Result is ObjectResult result && result.Value is EdmModel model)
        //        {
        //            var types = model.SchemaElements.Where(_ =>
        //                _.SchemaElementKind == EdmSchemaElementKind.TypeDefinition);

        //            var schema = new JsonSchema4();
        //            schema.Definitions = types.Select(_=> JsonSchema4.FromTypeAsync<>())

        //        }
        //    }
        //}

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //if (context.HttpContext.Request.Path.Value.Contains("$metadata")
            //    && context.HttpContext.Request.Query.ContainsKey("$format")
            //    && context.HttpContext.Request.Query["$format"].Count > 0
            //    && context.HttpContext.Request.Query["$format"][0] == "json")
            //{
            //    Console.WriteLine("OnResultExecuted");
            //}
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.HttpContext.Request.Path.Value.Contains("$metadata")
                && context.HttpContext.Request.Query.ContainsKey("$format")
                && context.HttpContext.Request.Query["$format"].Count > 0
                && context.HttpContext.Request.Query["$format"][0] == "json")
            {
                context.Cancel = true;
                if (context.Result is ObjectResult result && result.Value is EdmModel model)
                {
                    var edmSchemaElements = model.SchemaElements.Where(_ =>
                        _.SchemaElementKind == EdmSchemaElementKind.TypeDefinition);

                    var schema = new JsonSchema4();
                    foreach (var edmSchemaElement in edmSchemaElements)
                    {
                        var name = edmSchemaElement.Namespace + edmSchemaElement.Name;

                        if (edmSchemaElement is IEdmStructuredType structuredType)
                        {
                            var elementSchema = GetTypeSchema(structuredType);
                            schema.Definitions.Add(name, elementSchema);
                        }
                    }

                    context.HttpContext.Response.ContentType = "application/json;";
                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(schema));

                    // context.Result = new ObjectResult(schema);
                }
            }

            // await next();
        }

        private JsonSchema4 GetTypeSchema(IEdmStructuredType edmStructuredType)
        {
            var elementSchema = new JsonSchema4();
            foreach (var property in edmStructuredType.DeclaredProperties)
            {
                elementSchema.Properties.Add(property.Name, MapToJsonProperty(property.Type));
            }

            return elementSchema;
        }

        private JsonProperty MapToJsonProperty(IEdmTypeReference propertyType)
        {
            var jsonProperty = new JsonProperty();
            switch (propertyType.Definition.TypeKind)
            {
                case EdmTypeKind.None:
                    break;
                case EdmTypeKind.Primitive:
                    if (propertyType.Definition is IEdmPrimitiveType primitiveType)
                    {
                        switch (primitiveType.PrimitiveKind)
                        {
                            case EdmPrimitiveTypeKind.None:
                                jsonProperty.Type = JsonObjectType.None;
                                break;
                            case EdmPrimitiveTypeKind.Boolean:
                                jsonProperty.Type = JsonObjectType.Boolean;
                                break;
                            case EdmPrimitiveTypeKind.Byte:
                            case EdmPrimitiveTypeKind.Decimal:
                            case EdmPrimitiveTypeKind.Double:
                            case EdmPrimitiveTypeKind.SByte:
                            case EdmPrimitiveTypeKind.Single:
                                jsonProperty.Type = JsonObjectType.Number;
                                break;
                            case EdmPrimitiveTypeKind.Int16:
                            case EdmPrimitiveTypeKind.Int32:
                            case EdmPrimitiveTypeKind.Int64:
                                jsonProperty.Type = JsonObjectType.Integer;
                                break;
                            default:
                                jsonProperty.Type = JsonObjectType.String;
                                break;
                        }
                    }

                    break;
                case EdmTypeKind.Entity:
                    break;
                case EdmTypeKind.Complex:
                    if (propertyType.Definition is IEdmComplexType)
                    {
                        jsonProperty.Type = JsonObjectType.Object;
                    }

                    break;
                case EdmTypeKind.Collection:
                    jsonProperty.Type = JsonObjectType.Array;
                    break;
                case EdmTypeKind.EntityReference:
                    break;
                case EdmTypeKind.Enum:
                    break;
                case EdmTypeKind.TypeDefinition:
                    break;
                case EdmTypeKind.Untyped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return jsonProperty;
        }
    }
}
