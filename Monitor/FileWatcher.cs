using System;
using System.IO;

namespace filemon.Monitor
{
    public class Watcher
    {
        public string Path { get; set; }
        public FileSystemWatcher FileWatcher { get; set; }

        private NotifyFilters filters = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName 
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
        public NotifyFilters Filters { 
            get {return filters;} 
            set {filters = value;}    
        }
        public void  Run(){
 

            FileWatcher = new FileSystemWatcher(Path);

            FileWatcher.NotifyFilter = this.Filters;

            FileWatcher.Changed += OnChanged;
            FileWatcher.Created += OnCreated;
            FileWatcher.Deleted += OnDeleted;
            FileWatcher.Renamed += OnRenamed;
            FileWatcher.Error   += OnError;

            FileWatcher.IncludeSubdirectories = true;
            FileWatcher.EnableRaisingEvents = true; 
        }


        private void OnChanged(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);
        }
        private void OnCreated(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        private void OnDeleted(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        private void OnRenamed(object sender, RenamedEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        private void OnError(object sender,  ErrorEventArgs e){
            Console.WriteLine("File Error! "+e.GetException().Message);

        }
    }
}
