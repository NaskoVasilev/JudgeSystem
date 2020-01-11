using System;
using System.Collections.Generic;
using System.IO;

using JudgeSystem.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace JudgeSystem.Services
{
    public class JsonUtiltyService : IJsonUtiltyService
    {
        public T ParseJsonFormStreamUsingJSchema<T>(Stream stream, string schemaFilePath, List<string> messages)
        {
            using var streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();
            var reader = new JsonTextReader(new StringReader(json));

            var validatingReader = new JSchemaValidatingReader(reader);
            string schema = File.ReadAllText(schemaFilePath);
            validatingReader.Schema = JSchema.Parse(schema);

            validatingReader.ValidationEventHandler += (o, a) => messages.Add(a.Message);
            var serializer = new JsonSerializer();
            try
            {
                return serializer.Deserialize<T>(validatingReader);
            }
            catch (Exception)
            {
                messages.Add(ErrorMessages.InvalidJsonFile);
            }

            return default;
        }
    }
}
