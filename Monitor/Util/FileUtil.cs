using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;


namespace filemon.Util{


    public class FileUtil{

        public static async Task<byte[]> Read(string path){
            return await System.IO.File.ReadAllBytesAsync(path);
        }
        public static bool isDirectory(string path){
            DirectoryInfo di = new DirectoryInfo(path); 
            return di.Exists;
        }
        public static void createDirectory(string path){
            DirectoryInfo di = new DirectoryInfo(path); 
            di.Create();
        }
        public static FileStream GetStream(string path   ){
            FileStream fs = new FileStream(path , FileMode.OpenOrCreate,FileAccess.ReadWrite);
            return fs;
        }

        public static List<string> GetFiles(string path){
            DirectoryInfo di = new DirectoryInfo(path); 
            var files=  di.GetFiles().Select(x=>x.FullName.Replace(path,"")).ToList();
            return files;
        }
    }
}