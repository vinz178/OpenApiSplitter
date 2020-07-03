using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenApiSplitter
{
    public class ToSplitOpenApi
    {
        private readonly OpenApi _inputOpenApi;

        public ToSplitOpenApi(string sourceFile)
        {
            string jsonString;
            using (StreamReader streamReader = File.OpenText(sourceFile))
            {
                jsonString = streamReader.ReadToEnd();
            }
            OpenApi inputOpenApi = JsonConvert.DeserializeObject<OpenApi>(jsonString);
            _inputOpenApi = inputOpenApi;
        }

        public List<SplitOpenApi> SplitOpenApi()
        {
            
            List<SplitOpenApi> listOfSplitOpenApis = new List<SplitOpenApi>();

            foreach (KeyValuePair<string, Dictionary<string, JObject>> path in _inputOpenApi.Paths)
            {
                foreach (KeyValuePair<string, JObject> method in path.Value)
                {
                   
                    SplitOpenApi splitOpenApi = new SplitOpenApi(_inputOpenApi, method, path);
                    listOfSplitOpenApis.Add(splitOpenApi);
                }

            }

            return listOfSplitOpenApis;
        }
    }
}
