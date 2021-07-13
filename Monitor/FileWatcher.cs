using System;
using System.IO;
using System.Collections.Generic;

namespace filemon.Monitor
{
    public class Watcher
    {
        private static Watcher _instance;
        private Watcher(){
            Path = "/App";
            Handler = new ConsoleHandler();
            Filters = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName 
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            FileWatchers = new List<FileSystemWatcher>();
        }
        public static Watcher CreateWatcher(){
            if(_instance == null)
            {
                _instance = new Watcher();


            }
            return _instance;
        }
        public IChangeHandler Handler { get; set; }
        public string Path { get; set; }
        public List<FileSystemWatcher> FileWatchers { get; set; }
 
        public NotifyFilters Filters { get; set; }


        public void  Run(){
            
            FileWatchers.ForEach(fw=>{

                fw = new FileSystemWatcher(Path);

                fw.NotifyFilter = this.Filters;

                fw.Changed += Handler.OnChanged;
                fw.Created += Handler.OnCreated;
                fw.Deleted += Handler.OnDeleted;
                fw.Renamed += Handler.OnRenamed;
                fw.Error   += Handler.OnError;

                fw.IncludeSubdirectories = true;
                fw.EnableRaisingEvents = true; 
            });

        }


 
    }

}
