using System;
using filemon.Monitor;
using Figgle;
namespace filemon.Exe
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try{
                var arg_var = new filemon.Variable.ConsoleVariable(args);
                Watcher watcher = Watcher.CreateWatcher(arg_var); 

                Write(ConsoleColor.Cyan, "Welcome to");
                Write(ConsoleColor.Cyan , FiggleFonts.Standard.Render("DETEKT!"));

                watcher.GlobalVaribles.WarningCreated+=(WarningEventArgs a)=> Write(ConsoleColor.Yellow , String.Format("Warning: {0}",a.Message));
                watcher.GlobalVaribles.AskCreated+=(WarningEventArgs a)=> Ask(ConsoleColor.White , String.Format(a.Message));
                
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
        static string Ask(ConsoleColor col , string message){
            Console.ForegroundColor = col;
            Console.Out.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            return Console.ReadLine();
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
