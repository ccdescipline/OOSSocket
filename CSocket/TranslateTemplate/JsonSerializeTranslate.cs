using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace CSocket.TranslateTemplate
{
    /// <summary>
    /// json序列化类
    /// </summary>
    public class JsonSerializeTranslate : IBodyTranslate
    {
        JsonSerializerOptions option = new JsonSerializerOptions() {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        public JsonSerializeTranslate()
        {

        }

        public T Deserialize<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str, option);
        }

        public string serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj,option);
        }
    }
}
