using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenApiSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            //todo check argument destination and source
            string sourceFile = args[0];
            string implementingService = args[2]; 
            string destinationDir = args[1];


            ToSplitOpenApi toSplitOpenApi = new ToSplitOpenApi(sourceFile);
            List<SplitOpenApi> splitOpenApis = toSplitOpenApi.SplitOpenApi();
            foreach (SplitOpenApi splitOpenApi in splitOpenApis)
            {
                splitOpenApi.Save(destinationDir, implementingService);
            }
        }
    }
}
