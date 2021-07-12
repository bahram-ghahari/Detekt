using System;
using System.IO;

namespace filemon.Monitor
{
    public class Watcher
    {

        public Watcher(){
            Path = "/App";
            Handler = new ConsoleHandler();
            Filters = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName 
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
        }
        public IChangeHandler Handler { get; set; }
        public string Path { get; set; }
        public FileSystemWatcher FileWatcher { get; set; }
 
        public NotifyFilters Filters { get; set; }


        public void  Run(){
 

            FileWatcher = new FileSystemWatcher(Path);

            FileWatcher.NotifyFilter = this.Filters;

            FileWatcher.Changed += Handler.OnChanged;
            FileWatcher.Created += Handler.OnCreated;
            FileWatcher.Deleted += Handler.OnDeleted;
            FileWatcher.Renamed += Handler.OnRenamed;
            FileWatcher.Error   += Handler.OnError;

            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.EnableRaisingEvents = true; 
        }


 
    }
}
