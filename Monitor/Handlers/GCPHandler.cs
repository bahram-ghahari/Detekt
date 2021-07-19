using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Google;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1.Data;


namespace filemon.Monitor{
    public class GCPHandler : IChangeHandler
    { 
        public FilemonVariable GlobalVariable { get; set; }


        // The Google Cloud Storage client.
        readonly StorageClient _storage;

        public string Name { get{ return "GCP";}}

        public GCPHandler(){
            _storage = StorageClient.Create();
        }
        public void OnChanged(object sender, FileSystemEventArgs e){
            try{
                _storage.DeleteObject(GlobalVariable.Bucket,e.Name);
                var fs = filemon.Util.FileUtil.GetStream(e.FullPath); 
                _storage.UploadObject(GlobalVariable.Bucket,e.Name,"application/octet-stream",fs);

            }catch{}
        }
        public void OnCreated(object sender, FileSystemEventArgs e){
            if(!filemon.Util.FileUtil.isDirectory(e.Name)){
                var fs = filemon.Util.FileUtil.GetStream(e.FullPath);
                
                _storage.UploadObject(GlobalVariable.Bucket,e.Name,"application/octet-stream",fs);
            }
        }
        public void OnDeleted(object sender, FileSystemEventArgs e){
            try{
                
                var objects = _storage.ListObjects(GlobalVariable.Bucket);
                var list = objects.ToList();
                foreach (var item in list)
                {
                    if(item.Name.StartsWith(e.Name)){
                        _storage.DeleteObject(GlobalVariable.Bucket,item.Name);
                    }
                }

            }
            catch(Exception ex) {}
        }
        public void OnRenamed(object sender, RenamedEventArgs e){
            _storage.CopyObject(GlobalVariable.Bucket,e.OldName,GlobalVariable.Bucket,e.Name);
            _storage.DeleteObject(GlobalVariable.Bucket,e.OldName);
        }
        public void OnError(object sender,  ErrorEventArgs e){

        }

        public void OnStart(object sender,  EventArgs e){
 
            var option = new ListBucketsOptions(); 
            option.Prefix = GlobalVariable.Bucket;
            var list = _storage.ListBuckets(GlobalVariable.ProjectId,option);
         
           
           if(list.Count()>0){//BUCKET EXISTS
            

           }else{
            _storage.CreateBucket(GlobalVariable.ProjectId,GlobalVariable.Bucket);

           }
        }
        public void OnDestroy(object sender,  EventArgs e){

        }
    }
}
 