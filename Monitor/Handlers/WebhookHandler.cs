using System;
using System.IO;

namespace filemon.Monitor{
    public class WebhookHandler : IChangeHandler
    { 
        public string Name { get{return "WH";}}
        public void OnChanged(object sender, FileSystemEventArgs e){
             
        }
        public void OnCreated(object sender, FileSystemEventArgs e){ 

        }
        public void OnDeleted(object sender, FileSystemEventArgs e){ 

        }
        public void OnRenamed(object sender, RenamedEventArgs e){ 

        }
        public void OnError(object sender,  ErrorEventArgs e){ 

        }
    }
}