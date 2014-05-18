#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

#endregion

namespace Hoover.Helpers
{
    public static class Extenstions
    {
        /// <summary>
        /// Extension method for <see cref="JObject"/> when getting the value. Handles the situations when key is not found which returns the default value of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(this JObject jObject, string key, T defaultValue = default(T))
        {
            T retval = defaultValue;

            if (jObject != null)
            {
                var val = jObject[key];
                if (val != null) return val.Value<T>();
            }

            return retval;
        }
    }
}