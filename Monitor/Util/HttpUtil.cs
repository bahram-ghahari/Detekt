using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;


namespace filemon.Util{

    public class HttpUtil{
 

        public static async Task<string> Post(byte[] data, Dictionary<string , string> json , string url)
        { 
            HttpContent bytesContent = new ByteArrayContent(data);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                for (int i = 0; i < json.Count; i++)
                {
                    HttpContent stringContent = new StringContent(json.ElementAt(i).Key); 
                    
                    formData.Add(stringContent, json.ElementAt(i).Key, json.ElementAt(i).Value); 
                }
                formData.Add(bytesContent , "File" );
                var response = await client.PostAsync(url, formData);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}