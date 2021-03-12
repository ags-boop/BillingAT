using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using BillingEnd.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
#nullable disable

namespace BillingEnd.Models
{
    public static class TempDataHelper
    {
        // public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        // {
        //     tempData[key] = JsonSerializer.Serialize(value);//Almacena
        // }

        // public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        // {
        //     object o;
        //     tempData.TryGetValue(key, out o);
        //     return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        // }
        // public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        // {
        //     object o = tempData.Peek(key);
        //     return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        // }
        public static List<T> Deserialize<T>(this string SerializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);
            return stuff;
        }

        
    }
}