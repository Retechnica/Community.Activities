using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Net.Http;
using Newtonsoft.Json;


namespace IngeniaActivity
{
    public class IngeniaItemPost : CodeActivity
    {

        public OutArgument<string> IngeniaResponse { get; set; }

        //
        // These inputs are placeholders: currently manual inputs, they will need to be integrated in the workflow.
        //
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ApiKey { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> BundleId { get; set; }


        // Optional field, enables you to train the system. 
        // A string to be formatted exactly as below, with as many tags as needed:
        // ["tag1", "tag2", "tag3"]
        // the code then converts this into actual JSON
        [Category("Input")]
        public InArgument<string> Tags { get; set; }
        //
        //

        private static readonly HttpClient client = new HttpClient();

        protected override void Execute(CodeActivityContext context)
        {

            // Get the various inputs
            var text = Text.Get(context);
            var apiKey = ApiKey.Get(context);
            var bundleId = BundleId.Get(context);
            string tags = JsonConvert.SerializeObject(Tags.Get(context));

            // Build the item json
            var jsonValues = new Dictionary<string, dynamic>()
            {
                { "text", text },
                { "bundle_id", bundleId }
            };

            // If there are tags to be assigned, add this key/value to the json
            if (tags.Length > 0)
            {
                jsonValues["tags"] = tags;
            };

            // Serialize the json
            string json = JsonConvert.SerializeObject(jsonValues);

            // Build URL content (e.g. api_key etc.) and encode it correctly
            // classify = true requires Ingenia to return the classification results in real time.
            var urlContent = new Dictionary<string, string>
            {
                { "api_key", apiKey },
                { "classify", "true" },
                { "json", json }
            };
            var content = new FormUrlEncodedContent(urlContent);

            // Post to Ingenia Items URL to do item#create
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync("https://ingeniapi.com/v2/items", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        string responseString = responseContent.ReadAsStringAsync().Result;

                        //
                        // This output might need changing. Currently just writes to console and assigns OutArgument IngeniaResponse.
                        //
                        IngeniaResponse.Set(context, responseString);
                        // Console.WriteLine(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
