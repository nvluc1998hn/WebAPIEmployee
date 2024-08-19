using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Swagger
{
    public class SwaggerSchemaCustomFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Loại bỏ thuộc tính có SwaggerHideAttribute
            if (schema.Properties.Count > 0)
            {
                const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var memberList = context.Type.GetFields(bindingFlags).Cast<MemberInfo>().Concat(context.Type.GetProperties(bindingFlags));

                var excludedList = memberList.Where(m => m.GetCustomAttribute<SwaggerHideAttribute>() != null).Select(m => (m.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? m.Name.ToCamelCase()));

                foreach (var excludedName in excludedList)
                {
                    schema.Properties.Remove(excludedName);
                }
            }
        }
    }

    internal static class StringExtensions
    {
        internal static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
