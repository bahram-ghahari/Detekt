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



    public async Task UploadAsync()
    {
        // Google Cloud Platform project ID.
        const string projectId = "project-id-goes-here";

        // The name for the new bucket.
        const string bucketName = projectId + "-test-bucket";

        // Path to the file to upload
        const string filePath = @"C:\path\to\image.jpg";

        var newObject = new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = bucketName,
            Name = System.IO.Path.GetFileNameWithoutExtension(filePath),
            ContentType = "image/jpeg"
        };

        // read the JSON credential file saved when you created the service account
        var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(System.IO.File.ReadAllText(
            @"c:\path\to\service-account-credentials.json"));

        // Instantiates a client.
        using (var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(credential))
        {
            try
            {
                // Creates the new bucket. Only required the first time.
                // You can also create buckets through the GCP cloud console web interface
                //storageClient.CreateBucket(projectId, bucketName); 

                // Open the image file filestream
                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                { 
                    // set minimum chunksize just to see progress updating
                    var uploadObjectOptions = new Google.Cloud.Storage.V1.UploadObjectOptions
                    {
                        ChunkSize = Google.Cloud.Storage.V1.UploadObjectOptions.MinimumChunkSize
                    };
 

                    await storageClient.UploadObjectAsync(
                            newObject, 
                            fileStream,
                            uploadObjectOptions)
                        .ConfigureAwait(false);
                }

            }
            catch (Google.GoogleApiException e)
                when (e.Error.Code == 409)
            { 
            }
            catch (Exception e)
            { 
            }
        }
    }

    }
}