using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Weather_Widget.Models
{
    public static class WeatherAPI
    {
        private static readonly string _key = "039d992b6267c9d758a6ded74467c1c9";
        public static bool IsError { get; set; }
        public static async Task<RootObject> GetInfo(string cityName)
        {
           RootObject rt = null;
           using (HttpClient client = new HttpClient())
           {
                try
                {
                    var result = await client.GetAsync($"http://api.openweathermap.org/data/2.5/forecast/daily?q={cityName}&units=metric&APPID={_key}");
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