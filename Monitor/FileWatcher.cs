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
            Handlers = new List<IChangeHandler>(); 
            Filters = NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName  
                                 | NotifyFilters.Size;
            FileWatcher = new FileSystemWatcher();

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


            GlobalVaribles.Run();

            for (int i = 0; i < GlobalVaribles.Handler.Length; i++){
                var _created_handler = HandlerFactory.Create(GlobalVaribles.Handler[i]);
                _created_handler.GlobalVariable = this.GlobalVaribles;
                Handlers.Add(_created_handler);
            }

            FileWatcher = new FileSystemWatcher(GlobalVaribles.Path);
            FileWatcher.NotifyFilter = this.Filters;

            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.EnableRaisingEvents = true; 
    
            Handlers.ForEach(h=>{  
                h.OnStart(this,new EventArgs());
                FileWatcher.Changed += h.OnChanged;
                FileWatcher.Created += h.OnCreated;
                FileWatcher.Deleted += h.OnDeleted;
                FileWatcher.Renamed += h.OnRenamed;
                FileWatcher.Error   += h.OnError;
            });

        }


 
    }

}
