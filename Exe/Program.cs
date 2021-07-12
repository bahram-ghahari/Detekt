using System;
using filemon.Monitor;

namespace filemon.Exe
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Watcher watcher = new Watcher();
            watcher.Path = args.Length>0 ? args[0] : "/home/cleantie/Documents/git/filemon/Exe";
            Console.WriteLine("watcher started!");
            watcher.Run();
            var cmd = "";
            do{
                cmd = System.Console.ReadLine();

            }while(!checkCommand(cmd));
            
            watcher.FileWatcher.Dispose();
        }

        static bool checkCommand(string cmd){
            if(cmd.ToLower().Trim()!="stop"){
                Console.WriteLine("Invalid command! type stop to terminate");
                return false;
            }
            return true;

        }
    }
}
