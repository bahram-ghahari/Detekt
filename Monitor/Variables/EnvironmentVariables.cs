using System;
using System.Linq;


namespace filemon.Variable{

    public class DetektEnvironmentVariable:FilemonVariable{
 

        public DetektEnvironmentVariable(){
        }

        public override void Run(){



            var _handler = GetVariable("DETEKT_CHANGE_HANDLER");
            if(string.IsNullOrWhiteSpace(_handler))
                throw new ArgumentException("Environment variable 'DETEKT_CHANGE_HANDLER' is not set.");
            this.Handler = _handler.Split('|',StringSplitOptions.RemoveEmptyEntries);
            if(Handler.Length==0)
                throw new ArgumentException("Environment variable 'CHANGE_HANDLER' is not set.");
            for (int i = 0; i < this.Handler.Length; i++)
                if(!IsHandlerValid( this.Handler[i] ))
                    throw new ArgumentException(Handler[i]+" is not a valid DETEKT handler.");
            



            
            Path = GetVariable("DETEKT_PATH_TO_WATCH");
            if(string.IsNullOrWhiteSpace(Path))
                throw new ArgumentException("Environment variable 'DETEKT_PATH_TO_WATCH' is not set.");






            ContainerId = GetVariable("DETEKT_CONTAINER_ID");
            if(string.IsNullOrWhiteSpace(ContainerId)){
                ContainerId = "con-";
                var rand = new Random();
                var random_number = rand.Next(1000000,9999999);
                ContainerId+=random_number;
                Warn("Environment variable 'DETEKT_CONTAINER_ID' is not set. '"+ContainerId+"' was generated randomly as the container name."); 

            }





            var WebHookSelected = Handler.Count(x=>x.ToUpper().Trim() == WH)>0;
            var GcpSelected = Handler.Count(x=>x.ToUpper().Trim() == GCP)>0;


            Bucket = GetVariable("DETEKT_GCP_BUCKET");
            if(GcpSelected){
                if(string.IsNullOrWhiteSpace(Bucket)){
                    Bucket = ContainerId; 
                    Warn("v 'DETEKT_GCP_BUCKET' is not set. '"+ContainerId+"' was generated randomly as the bucket name."); 
                }
                ProjectId = GetVariable("DETEKT_GCP_PROJECT_ID");
                if(string.IsNullOrWhiteSpace(ProjectId)){
                    throw new ArgumentException("Environment variable 'DETEKT_GCP_PROJECT_ID' is not set.");
                } 
            }



            OnChangedWebHook = GetVariable("DETEKT_WH_ON_CHANGED");
            OnCreatedWebHook = GetVariable("DETEKT_WH_ON_CREATED");
            OnRenamedWebHook = GetVariable("DETEKT_WH_ON_RENAMED");
            OnDeletedWebHook = GetVariable("DETEKT_WH_ON_DELETED");
            WebHookSignature = GetVariable("DETEKT_WH_SIGNATURE");

            if(WebHookSelected){
                if(string.IsNullOrWhiteSpace( OnChangedWebHook ))
                    Warn("'WH' Handler is selected, However environment variable 'DETEKT_WH_ON_CHANGED' has not been set. File changes will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnDeletedWebHook ))
                    Warn("'WH' Handler is selected, However environment variable 'DETEKT_WH_ON_DELETED' has not been set. File deletions will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnRenamedWebHook ))
                    Warn("'WH' Handler is selected, However environment variable 'DETEKT_WH_ON_RENAMED' has not been set. File name changes will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnCreatedWebHook ))
                    Warn("'WH' Handler is selected, However environment variable 'DETEKT_WH_ON_CREATED' has not been set. New files will not be captured."); 
                
            }

        }


        private string GetVariable(string name){
            return Environment.GetEnvironmentVariable(name);
        }
    } 


}