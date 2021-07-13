using System;
using System.Linq;


namespace filemon.Variable{

    public class FilemonEnvironmentVariables:FilemonVariable{
 

        public FilemonEnvironmentVariables(){
        }

        public override void Run(){



            var _handler = GetVariable("CHANGE_HANDLER");
            this.Handler = _handler.Split('|',StringSplitOptions.RemoveEmptyEntries);
            if(Handler.Length==0)
                throw new ArgumentException("Variable 'CHANGE_HANDLER' is not set.");
            for (int i = 0; i < this.Handler.Length; i++)
                if(!IsHandlerValid( this.Handler[i] ))
                    throw new ArgumentException(Handler[i]+" is not a valid filemon handler.");
            



            
            Path = GetVariable("PATH_TO_WATCH");
            if(string.IsNullOrWhiteSpace(Path))
                throw new ArgumentException("Variable 'PATH_TO_WATCH' is not set.");






            ContainerId = GetVariable("CONTAINER_ID");
            if(string.IsNullOrWhiteSpace(ContainerId)){
                ContainerId = "CON-";
                var rand = new Random();
                var random_number = rand.Next(1000000,9999999);
                ContainerId+=random_number;
                Warn("Variable 'CONTAINER_ID' is not set. '"+ContainerId+"' was generated randomly as the container name."); 

            }




            var WebHookSelected = Handler.Count(x=>x.ToUpper().Trim() == WH)>0;

            OnChangedWebHook = GetVariable("WH_ON_CHANGED");
            OnCreatedWebHook = GetVariable("WH_ON_CREATED");
            OnRenamedWebHook = GetVariable("WH_ON_RENAMED");
            OnDeletedWebHook = GetVariable("WH_ON_DELETED");
            WebHookSignature = GetVariable("WH_SIGNATURE");

            if(WebHookSelected){
                if(string.IsNullOrWhiteSpace( OnChangedWebHook ))
                    Warn("'WH' Handler is selected, However 'WH_ON_CHANGED' has not been set. File changes will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnDeletedWebHook ))
                    Warn("'WH' Handler is selected, However 'WH_ON_DELETED' has not been set. File deletions will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnRenamedWebHook ))
                    Warn("'WH' Handler is selected, However 'WH_ON_RENAMED' has not been set. File name changes will not be captured."); 
                if(string.IsNullOrWhiteSpace( OnCreatedWebHook ))
                    Warn("'WH' Handler is selected, However 'WH_ON_CREATED' has not been set. New files will not be captured."); 
                
            }

        }


        private string GetVariable(string name){
            return Environment.GetEnvironmentVariable(name);
        }
    } 


}