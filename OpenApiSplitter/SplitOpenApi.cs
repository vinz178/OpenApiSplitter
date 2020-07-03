using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenApiSplitter
{
    public class SplitOpenApi
    {
        private List<ToAddReference> ToAddReferences { get; } = new List<ToAddReference>();

        public SplitOpenApi(OpenApi unSplitApi, in KeyValuePair<string, JObject> method, in KeyValuePair<string, Dictionary<string, JObject>> path)
        {
            OpenApi = new OpenApi();
            OpenApi.OpenApiVersion = unSplitApi.OpenApiVersion;
            OpenApi.Info = unSplitApi.Info;
            OpenApi.Servers = unSplitApi.Servers;
            OpenApi.Security = unSplitApi.Security;
            OpenApi.Tags = unSplitApi.Tags;
            OpenApi.ExternalDocs = unSplitApi.ExternalDocs;

            Dictionary<string, JObject> newMethod = new Dictionary<string, JObject>();
            newMethod.Add(method.Key, method.Value);

            OpenApi.Paths = new Dictionary<string, Dictionary<string, JObject>>();
            OpenApi.Paths.Add(path.Key, newMethod);

            Method = method.Key;
            Path = path.Key;

            string serializedMethod = JsonConvert.SerializeObject(method.Value);
            SearchAndAddRefs(serializedMethod);

            AddReferencesToOpenApi(unSplitApi);

            //todo check with swagger diff
        }

        private void SearchAndAddRefs(string toSearchIn)
        {
            string pattern = "\"\\$ref\":.+?\"";
            var matches = Regex.Matches(toSearchIn, pattern);

            

            foreach (Match match in matches)
            {
                string reference = Regex.Replace(match.Value, "\"\\$ref\":", String.Empty)
                    .Replace("\"", "").Replace(" ", "").Replace("#", "");
                new ToAddReference(reference, this);
            }

            
        }

        public OpenApi OpenApi { get; set; }

        public string Method { get; set; }
        public string Path { get; set; }


        
        public void Save(string destinationDir, string implementingService)
        {
            string newSerializedOpenApi = JsonConvert.SerializeObject(OpenApi, Formatting.Indented);
            using (StreamWriter streamWriter = new StreamWriter(destinationDir + "/OpenApi_" + implementingService + "_" + Method + "-" + Path.Replace("/", "-") + ".json"))
            {
                streamWriter.Write(newSerializedOpenApi);
            }
        }

        public void AddReferencesToOpenApi(OpenApi unSplitApi)
        {
            Dictionary<string, JObject> newComponent = new Dictionary<string, JObject>();
            while (ToAddReferences.Any(r => !r.IsAddedToOpenApi))
            {
                ToAddReference toAddReference = ToAddReferences.First(r => !r.IsAddedToOpenApi);

                string[] toAddReferenceSplit = toAddReference.Reference.Split("/");
                int node = 1;
                if (toAddReferenceSplit[node] == "components")
                {
                    newComponent = TreatNextNode(unSplitApi.Components, newComponent, toAddReferenceSplit, node);
                }

                toAddReference.IsAddedToOpenApi = true;
            }

            OpenApi.Components = newComponent;
        }

        private Dictionary<string, JObject> TreatNextNode(Dictionary<string, JObject> oldDictionary, Dictionary<string, JObject> newDictionary, string[] refSplit, int node)
        {

            node++;
            if (newDictionary == null)
            {
                newDictionary = new Dictionary<string, JObject>();
            }
            if (!newDictionary.ContainsKey(refSplit[node]))
            {
                newDictionary.Add(refSplit[node], null);
            }
            if (refSplit.Length - 1 <= node)
            {

                newDictionary[refSplit[node]] = oldDictionary[refSplit[node]];

                string serializedRef = JsonConvert.SerializeObject(oldDictionary[refSplit[node]]);
                SearchAndAddRefs(serializedRef);
                return newDictionary;

            }
            string oldJObjectSerialized = JsonConvert.SerializeObject(oldDictionary[refSplit[node]]);
            string newJObjectSerialized = JsonConvert.SerializeObject(newDictionary[refSplit[node]]);

            Dictionary<string, JObject> oldNextDictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(oldJObjectSerialized);
            Dictionary<string, JObject> newNextDictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(newJObjectSerialized);
            newNextDictionary = TreatNextNode(oldNextDictionary, newNextDictionary, refSplit, node);
            string newNextJObjectSerialized = JsonConvert.SerializeObject(newNextDictionary);
            JObject newNextJObjectDeserialized = JsonConvert.DeserializeObject<JObject>(newNextJObjectSerialized);
            newDictionary[refSplit[node]] = newNextJObjectDeserialized;
            return newDictionary;
        }

        private class ToAddReference
        {
            public bool IsAddedToOpenApi { get; set; }
            public string Reference { get; }

            public ToAddReference(string reference, SplitOpenApi splitOpenApi)
            {
                var splitOpenApi1 = splitOpenApi;
                if (splitOpenApi1.ToAddReferences.Any(r =>r.Reference == reference))
                {
                    return;
                }
                Reference = reference;
                IsAddedToOpenApi = false;
                splitOpenApi1.ToAddReferences.Add(this);
            }

            
        }
    }
}
