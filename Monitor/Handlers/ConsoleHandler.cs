using System;
using System.IO;

namespace filemon.Monitor{
    public class ConsoleHandler : IChangeHandler
    { 
        public string Name { get{ return "CON";}}
        public void OnChanged(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);
        }
        public void OnCreated(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        public void OnDeleted(object sender, FileSystemEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        public void OnRenamed(object sender, RenamedEventArgs e){
            Console.WriteLine("File "+e.ChangeType.ToString()+". Name: "+e.Name);

        }
        public void OnError(object sender,  ErrorEventArgs e){
            Console.WriteLine("File Error! "+e.GetException().Message);

        }
    }
}