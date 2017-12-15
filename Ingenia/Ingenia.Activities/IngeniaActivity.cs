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
        // These inputs might need changing, as they are currently just manual inputs.
        //
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Api_key { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> Bundle_id { get; set; }


        // Optional. To be formatted exactly as below, with as many tags as needed:
        // ["tag1", "tag2", "tag3"]
        [Category("Input")]
        public InArgument<string> Tags { get; set; }
        //
        //
        //

        private static readonly HttpClient client = new HttpClient();

        protected override void Execute(CodeActivityContext context)
        {

            // Get the various inputs
            var text = Text.Get(context);
            var api_key = Api_key.Get(context);
            var bundle_id = Bundle_id.Get(context);
            string tags = JsonConvert.SerializeObject(Tags.Get(context));

            // Build the item json
            var json_values = new Dictionary<string, dynamic>()
            {
                { "text", text },
                { "bundle_id", bundle_id }
            };

            // If there are tags to be assigned, add this key/value to the json
            if (tags.Length > 0)
            {
                json_values["tags"] = tags;
            };

            // Serialize the json
            string json = JsonConvert.SerializeObject(json_values);

            // Build URL content (e.g. api_key etc.) and encode it correctly
            var url_content = new Dictionary<string, string>
            {
                { "api_key", api_key },
                { "classify", "true" },
                { "json", json }
            };
            var content = new FormUrlEncodedContent(url_content);

            // Post to Ingenia Items URL to do item#create
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
    }
}
