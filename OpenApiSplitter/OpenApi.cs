using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace OpenApiSplitter
{
    public class OpenApi
    {
        [JsonProperty("openapi")] public string OpenApiVersion { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public Info Info { get; set; }

        [JsonProperty("servers", NullValueHandling = NullValueHandling.Ignore)]
        public Server[] Servers { get; set; }


        [JsonProperty("paths", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, JObject>> Paths { get; set; }

        [JsonProperty("components", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JObject> Components { get; set; }

        [JsonProperty("security", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string[]> Security { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public Tag[] Tags { get; set; }

        [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
        public ExternalDocs ExternalDocs { get; set; }
    }


    public class Server
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("variables", NullValueHandling = NullValueHandling.Ignore)]
        public JObject Variables { get; set; }
    }

    public class ExternalDocs
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public class Info
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("termsOfService", NullValueHandling = NullValueHandling.Ignore)]
        public string TermsOfService { get; set; }

        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }

        [JsonProperty("license", NullValueHandling = NullValueHandling.Ignore)]
        public License License { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }
    }

    public class Contact
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }

    public class License
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }


    public class Tag
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
        public ExternalDocs ExternalDocs { get; set; }
    }
}