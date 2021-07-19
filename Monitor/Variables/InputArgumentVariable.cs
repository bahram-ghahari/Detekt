using System;
using System.Linq;
using System.Collections.Generic;

namespace filemon.Variable{

    public class InputArgumentVariables:FilemonVariable{
 


        private List<ArgumentBit> ArgIndicator =  new List<ArgumentBit> ();
        private string[] args;

        public InputArgumentVariables(string[] _args){
            this.args = _args;
            this.SetArgIndicator();
        }
        public override void Run(){

            if(!CheckPlacements(args))
                throw new ArgumentException("Some arguments seems to be invalid.");

            var q = this.QueArgs(args);

            while(q.Count>0){
                var arg = q.Dequeue();
                arg = arg.Trim().ToLower();
                var bit = ArgIndicator.FirstOrDefault(x=>x.ShortArg==arg || x.LongArg == arg);
                if(bit != null){
                    var arg_value = q.Dequeue();
                    bit.Func.Invoke(arg_value);
                }
            }
            


            if(Handler==null || Handler.Length==0)
                throw new ArgumentException("Variable '-h' is not set.");
            for (int i = 0; i < this.Handler.Length; i++)
                if(!IsHandlerValid( this.Handler[i] ))
                    throw new ArgumentException(Handler[i]+" is not a valid filemon handler.");
            



             
            if(string.IsNullOrWhiteSpace(Path))
                throw new ArgumentException("Variable '-p' is not set.");





 
            if(string.IsNullOrWhiteSpace(ContainerId)){
                ContainerId = "con-";
                var rand = new Random();
                var random_number = rand.Next(1000000,9999999);
                ContainerId+=random_number;
                Warn("Variable '-c' is not set. '"+ContainerId+"' was generated randomly as the container name."); 

            }




            var WebHookSelected = Handler.Count(x=>x.ToUpper().Trim() == WH)>0;
            var GcpSelected = Handler.Count(x=>x.ToUpper().Trim() == GCP)>0;
            if(GcpSelected){
                if(string.IsNullOrWhiteSpace(Bucket)){
                    Bucket = ContainerId; 
                    Warn("Variable 'GCP_BUCKET' is not set. '"+ContainerId+"' was generated randomly as the bucket name."); 
                }
            }


            if(WebHookSelected){
                if(string.IsNullOrEmpty( OnChangedWebHook ))
                    Warn("'WH' Handler is selected, However '-wh-ch' has not been set. File changes will not be captured."); 
                if(string.IsNullOrEmpty( OnDeletedWebHook ))
                    Warn("'WH' Handler is selected, However '-wh-de' has not been set. File deletions will not be captured."); 
                if(string.IsNullOrEmpty( OnRenamedWebHook ))
                    Warn("'WH' Handler is selected, However '-wh-rn' has not been set. File name changes will not be captured."); 
                if(string.IsNullOrEmpty( OnCreatedWebHook ))
                    Warn("'WH' Handler is selected, However '-wh-cr' has not been set. New files will not be captured."); 
            }

        }

        private void SetArgIndicator(){
            /*
            {"-p","--path"},
            {"-c","--container-id"},
            {"-h","--handlers"},
            {"-wh-ch","--wh-on-changed"},
            {"-wh-cr","--wh-on-created"},
            {"-wh-rn","--wh-on-renamed"},
            {"-wh-de","--wh-on-deleted"},
            {"-wh-sig","--wh-signature"},
            {"-gcp-b","--gcp-bucket"},

*/
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "-p",
                    LongArg="--path",
                    Description="Path to watch" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid path");
                        Path = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "-c",
                    LongArg="--container-id",
                    Description="Container id to indicate the source" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid container id");
                        ContainerId = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "-h",
                    LongArg="--handlers",
                    Description="use on or combination of 'CON' for console|'WH' fpr web hooks|'GCP' for GCP storage bucket|'AWS' for S3" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid handler");
                        Handler = str.Split('|',StringSplitOptions.RemoveEmptyEntries);
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "-wh-ch",
                    LongArg="--wh-on-changed",
                    Description="Web hook for changed files and folders" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid Web hook value");
                        OnChangedWebHook = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "wh-cr",
                    LongArg="--wh-on-created",
                    Description="Web hooks for created files and folders" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid Web hook value");
                        OnCreatedWebHook = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "wh-rn",
                    LongArg="--wh-on-renamed",
                    Description="Web hooks for renamed files and folders" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid Web hook value");
                        OnRenamedWebHook = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "wh-de",
                    LongArg="--wh-on-deleted",
                    Description="Web hooks for deleted files and folders" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid Web hook value");
                        OnDeletedWebHook = str;
                        return str;
                    }
                }
            );
            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "wh-sig",
                    LongArg="--wh-signature",
                    Description="Web hook signature for extra security" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid Web hook value");
                        WebHookSignature = str;
                        return str;
                    }
                }
            );

            ArgIndicator.Add( new  ArgumentBit{ 
                    ShortArg = "gcp-b",
                    LongArg="--gcp-bucket",
                    Description="Storage bucket name for Google Cloud Platform" , 
                    Func=(string str)=>{
                        if(str.StartsWith("-"))throw new ArgumentException("Invalid bucket name");
                        Bucket = str;
                        return str;
                    }
                }
            );
        } 
        private Queue<string> QueArgs(string[] args)=>  new Queue<string>(args);
        private bool CheckPlacements(string[] args){
            for (int i = 0; i < args.Length; i++)
                if(i%2==0 && args[i].IndexOf("-")==-1) return false;
            return true;
        }
    } 

    public class ArgumentBit{
        public string ShortArg { get; set; }
        public string LongArg { get; set; }
        public string Description { get; set; }
        public Func<string,string> Func { get; set;}
    }
}