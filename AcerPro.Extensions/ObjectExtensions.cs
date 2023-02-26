using Newtonsoft.Json;
using System;

namespace AcerPro.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
