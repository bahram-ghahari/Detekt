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
            var w = new Watcher();
            var _h = new _Handler();
            w.Handler =_h;

            var path = Directory.GetCurrentDirectory();
            
            w.Path = path;

            w.Run();

            Directory.CreateDirectory(path+"/sample");
            var res = await _h.Get<FileSystemEventArgs>();
            Assert.True(res!=null,"file monitor object id not null");
            Assert.True(res.Name == "sample" , "file name is sample");


            Directory.Delete(path+"/sample");

        }
    }


    class _Handler:IChangeHandler{
        private FileSystemEventArgs a1;
        private RenamedEventArgs a2;
        private ErrorEventArgs a3;


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
