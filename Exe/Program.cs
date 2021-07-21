using System;
using filemon.Monitor;

namespace filemon.Exe
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try{
                var arg_var = new filemon.Variable.FilemonEnvironmentVariables();
                Watcher watcher = Watcher.CreateWatcher(arg_var); 
                
                watcher.GlobalVaribles.WarningCreated+=(WarningEventArgs a)=> Write(ConsoleColor.Yellow , String.Format("Warning: {0}",a.Message));
                watcher.Run();


                Write( ConsoleColor.Green , "DETEKT started!");


                var cmd = "";
                do{
                    Console.ForegroundColor = ConsoleColor.White;
                    cmd = System.Console.ReadLine();
                }while(!checkCommand(cmd));
                
                watcher.FileWatcher.Dispose();

            }
            catch(Exception e)
            {
                Write( ConsoleColor.DarkRed , string.Format("Error: {0}",e.Message ));
            }
        }

        static void Write(ConsoleColor col , string message){
                Console.ForegroundColor = col;
                Console.Out.WriteLine(message);
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
