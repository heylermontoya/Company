﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace Company.Api.Filters
{
    public class BasePathFilter : IDocumentFilter
    {
        private readonly string basePath;

        public BasePathFilter(string basePath)
        {
            this.basePath = basePath;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Servers.Add(new() { Url = basePath });

            var pathsToModify = new List<string>(
                swaggerDoc.Paths.Keys.Where(path => path.StartsWith(basePath))
            );

            foreach (string pathKey in pathsToModify)
            {
                string newKey = Regex.Replace(pathKey, $"^{basePath}", string.Empty);
                var pathValue = swaggerDoc.Paths[pathKey];

                swaggerDoc.Paths.Remove(pathKey);
                swaggerDoc.Paths.Add(newKey, pathValue);
            }
        }
    }
}