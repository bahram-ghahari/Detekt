using System;
using Xunit;
using filemon.Monitor;
using System.IO;
using System.Threading.Tasks;

namespace test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            

            var path = Directory.GetCurrentDirectory();
            string name = "sample";
            string dir_name = Path.Combine(path,name);

            var v = new _Variable();
            v.ContainerId="con-234";
            v.Path = path;
            v.Handler = new string[]{"CON"};
            var w =  Watcher.CreateWatcher(v);
        

            w.Run();

            Directory.CreateDirectory(dir_name);  

            Directory.Delete(dir_name);
             
        }
    }


    class _Variable:FilemonVariable{

        public override void Run(){

        }
    }
    class _Handler:IChangeHandler{
        public FilemonVariable GlobalVariable { get; set; }

        private FileSystemEventArgs a1;
        private RenamedEventArgs a2;
        private ErrorEventArgs a3;
        public string Name { get{return "TEST";}}

  

        public void OnChanged(object sender, FileSystemEventArgs e){
            a1 = e;
        }
        public void OnCreated(object sender, FileSystemEventArgs e){
            a1 = e; 
        }
        public void OnDeleted(object sender, FileSystemEventArgs e){
            a1 = e; 
        }
        public void OnRenamed(object sender, RenamedEventArgs e){
            a2 = e; 
        }
        public void OnError(object sender,  ErrorEventArgs e){
            a3 = e; 
        }
        public async Task<T> Get<T>()
        {
            System.Threading.Thread.Sleep(2000);//wait for 1 second and check flags
            if(typeof(T) == typeof(FileSystemEventArgs ))
                return (T) Convert.ChangeType(a1, typeof(T));
            else if(typeof(T) == typeof(RenamedEventArgs ))
                return (T) Convert.ChangeType(a2, typeof(T));
            else if(typeof(T) == typeof(ErrorEventArgs ))
                return (T) Convert.ChangeType(a3, typeof(T));
            else throw new NotImplementedException();
        }
    }
}
