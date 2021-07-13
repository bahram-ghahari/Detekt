using System;
using System.IO;
using System.Threading.Tasks;


namespace filemon.Util{


    public class FileUtil{

        public static async Task<byte[]> Read(string path){
            return await System.IO.File.ReadAllBytesAsync(path);
        }
        public static bool isDirectory(string path){
            DirectoryInfo di = new DirectoryInfo(path);
            return di.Exists;
        }
    }
}