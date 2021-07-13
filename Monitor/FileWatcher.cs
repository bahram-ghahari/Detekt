using System;
using System.IO;
using System.Collections.Generic;

namespace filemon.Monitor
{
    public class Watcher
    {
        private static Watcher _instance;
        private Watcher(FilemonVariable var){
            GlobalVaribles = var; 
            GlobalVaribles.Run();
            Handlers = new List<IChangeHandler>(); 
            Filters = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName 
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            FileWatcher = new FileSystemWatcher();

            for (int i = 0; i < GlobalVaribles.Handler.Length; i++)
                Handlers.Add(HandlerFactory.Create(GlobalVaribles.Handler[i]));
            
        }
        public static Watcher CreateWatcher(FilemonVariable _var){
            
            if(_instance == null)
            {
                _instance = new Watcher(_var); 
            }
            return _instance;
        }
        public List<IChangeHandler> Handlers { get; set; }
         
        public FilemonVariable GlobalVaribles { get; set; }
        public FileSystemWatcher FileWatcher  { get; set; }
 
        public NotifyFilters Filters { get; set; }


        public void  Run(){
            FileWatcher = new FileSystemWatcher(GlobalVaribles.Path);
            FileWatcher.NotifyFilter = this.Filters;

            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.EnableRaisingEvents = true; 
 
            Handlers.ForEach(h=>{  
                FileWatcher.Changed += h.OnChanged;
                FileWatcher.Created += h.OnCreated;
                FileWatcher.Deleted += h.OnDeleted;
                FileWatcher.Renamed += h.OnRenamed;
                FileWatcher.Error   += h.OnError;
            });

        }


 
    }

}
