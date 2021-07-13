using System;
using System.IO;
using filemon.Util;
using System.Collections.Generic;


namespace filemon.Monitor{
    public class WebhookHandler : IChangeHandler
    { 
        public FilemonVariable GlobalVariable { get; set; }
        public string Name { get{return "WH";}}
        public void OnChanged(object sender, FileSystemEventArgs e){
            if(GlobalVariable!=null && !string.IsNullOrEmpty( GlobalVariable.OnChangedWebHook) ){
                try{
                    this.Send(e.ChangeType , e.FullPath , e.Name ,  GlobalVariable.OnChangedWebHook);
                }catch{}
            }
        }
        public void OnCreated(object sender, FileSystemEventArgs e){ 
            if(GlobalVariable!=null && !string.IsNullOrEmpty( GlobalVariable.OnCreatedWebHook) ){
                try{
                    this.Send(e.ChangeType , e.FullPath , e.Name ,  GlobalVariable.OnCreatedWebHook);
                }catch{}
            }
        }
        public void OnDeleted(object sender, FileSystemEventArgs e){ 
            if(GlobalVariable!=null && !string.IsNullOrEmpty( GlobalVariable.OnDeletedWebHook) ){
                try{
                    this.Send(e.ChangeType , e.FullPath , e.Name ,  GlobalVariable.OnDeletedWebHook);
                }catch{}
            }
        }
        public void OnRenamed(object sender, RenamedEventArgs e){ 
            if(GlobalVariable!=null && !string.IsNullOrEmpty( GlobalVariable.OnRenamedWebHook) ){
                try{
                    this.Send(e.ChangeType , e.FullPath , e.OldName ,  GlobalVariable.OnRenamedWebHook);
                }catch{}
            }
        }
        public void OnError(object sender,  ErrorEventArgs e){ 

        }




        private async void Send(WatcherChangeTypes type , string path , string name ,string url , string oldName="" , string oldPath = ""){
            var is_dir = FileUtil.isDirectory(path);
            var json = new Dictionary<string , string >();
            json.Add("type",is_dir?"Folder":"File");
            json.Add("time_stamp",DateTime.UtcNow.ToString());
            json.Add("event_type",type.ToString());
            json.Add("name",name);
            json.Add("path",path);
            json.Add("signature",GlobalVariable.WebHookSignature);
            json.Add("container_name",GlobalVariable.ContainerId);

            var data = new byte[0];

            
            if(type== WatcherChangeTypes.Changed || type == WatcherChangeTypes.Created){
                if(!is_dir)
                    data = await FileUtil.Read(path); 
            }
            if(type == WatcherChangeTypes.Renamed){
                json.Add("old_name",oldName);
                json.Add("old_path",oldPath);
            }

            await HttpUtil.Post(data,json,url);
        }
    }
}