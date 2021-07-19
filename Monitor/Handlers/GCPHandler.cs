using System;
using System.IO;
using Google;
using Google.Cloud.Storage.V1;

namespace filemon.Monitor{
    public class GCPHandler : IChangeHandler
    { 
        public FilemonVariable GlobalVariable { get; set; }


        // The Google Cloud Storage client.
        readonly StorageClient _storage;

        public string Name { get{ return "CON";}}

        public GCPHandler(){
            _storage = StorageClient.Create();
        }
        public void OnChanged(object sender, FileSystemEventArgs e){
            try{
                _storage.DeleteObject("filemon",e.Name);
                var fs = filemon.Util.FileUtil.GetStream(e.FullPath); 
                _storage.UploadObject("filemon",e.Name,"application/octet-stream",fs);

            }catch{}
        }
        public void OnCreated(object sender, FileSystemEventArgs e){
            if(!filemon.Util.FileUtil.isDirectory(e.Name)){
                var fs = filemon.Util.FileUtil.GetStream(e.FullPath);
                
                _storage.UploadObject("filemon",e.Name,"application/octet-stream",fs);
            }
        }
        public void OnDeleted(object sender, FileSystemEventArgs e){
            try{
                _storage.DeleteObject("filemon",e.Name);
            }catch{}
        }
        public void OnRenamed(object sender, RenamedEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);
        }
        public void OnError(object sender,  ErrorEventArgs e){

        }


    }
}
 