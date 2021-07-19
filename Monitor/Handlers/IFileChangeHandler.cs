using System;
using System.IO;

namespace filemon.Monitor{
    public interface IChangeHandler
    {
        FilemonVariable GlobalVariable { get; set; }

        String  Name{get;}
        void OnChanged(object sender, FileSystemEventArgs e);
        void OnCreated(object sender, FileSystemEventArgs e);
        void OnDeleted(object sender, FileSystemEventArgs e);
        void OnRenamed(object sender, RenamedEventArgs e);
        void OnError(object sender,  ErrorEventArgs e);
    }

    public class HandlerFactory{
        public static IChangeHandler Create(string name){
            switch(name){
                case "WH": return new WebhookHandler();
                case "CON": return new ConsoleHandler();

                case "GCP": return new GCPHandler();
                case "AWS": throw new NotImplementedException();

                default: throw new ArgumentException();

            }
        }
    }
}