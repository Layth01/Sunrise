using Android.App;
using Android.Widget;
using Android.OS;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System;

namespace Sunrise
{
    [Activity(Label = "Sunrise", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.getWeatherButton);
            var editText1 = FindViewById<EditText>(Resource.Id.editText1);
            var textView1 = FindViewById<TextView>(Resource.Id.textView1);
            button.Click += async (Sender, e) =>
            {
                button.Enabled = false;
                string url = "https://query.yahooapis.com/v1/public/yql?q=select%20astronomy.sunset%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + editText1.Text + "%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
                JsonValue json = await FetchWeatherAsync(url);
              //  JsonValue weatherResults = json["query"]["results"];
                string weatherResults = json["query"]["results"]["channel"]["astronomy"]["sunset"].ToString();
                textView1.Text = weatherResults;
                button.Enabled = true;
            };
        }


        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }
    }
}
 



