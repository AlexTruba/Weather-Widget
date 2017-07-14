using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Weather_Widget.Models
{
    public class WeatherAPI: IWeather
    {
        public string ApiKey { get; set; } 
        public string BaseUrl { get; set; }
        public bool IsError { get; set; }
        public WeatherAPI()
        {
            ApiKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];
            BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
        }

        public async Task<RootObject> GetInfoAsync(string cityName)
        {
           RootObject rt = null;
           using (HttpClient client = new HttpClient())
           {
                try
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    var result = await client.GetAsync($"?q={cityName}&units=metric&APPID={ApiKey}");
                    if (result.IsSuccessStatusCode)
                    {
                        IsError = false;
                        string info = await result.Content.ReadAsStringAsync();
                        rt = JsonConvert.DeserializeObject<RootObject>(info);
                    }
                    else
                    {
                        IsError = true;
                    }
                }
                catch (Exception)
                {
                    IsError = true;
                }
            }
            return rt;
        }
    }
}