using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;

namespace Finance_Manager_Backend.Middleware;

public class JsonRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || context.Type == null)
            return;

        foreach (var prop in context.Type.GetProperties())
        {
            var jsonPropAttr = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                   .FirstOrDefault() as JsonPropertyAttribute;

            if (jsonPropAttr != null && jsonPropAttr.Required == Required.Always)
            {
                var name = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                if (schema.Properties.ContainsKey(name))
                {
                    schema.Properties[name].Nullable = false;
                }

                if (!schema.Required.Contains(name))
                {
                    schema.Required.Add(name);
                }
            }
        }
    }
}
