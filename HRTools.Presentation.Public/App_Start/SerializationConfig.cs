using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HRTools.Presentation.Public
{
    public static class SerializationConfig
    {
        internal static void ConfigureJsonSerializer()
        {
            JsonMediaTypeFormatter formatter =
                GlobalConfiguration.Configuration.Formatters.FirstOrDefault(
                    x => x.SupportedMediaTypes.Any(y => string.Equals(y.MediaType, "application/json", StringComparison.CurrentCultureIgnoreCase))) as
                JsonMediaTypeFormatter;

            if (formatter != null)
            {
                formatter.SerializerSettings = CreateSettings();
            }

            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(formatter);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.TypeNameHandling = TypeNameHandling.None;

            return settings;
        }
    }
}