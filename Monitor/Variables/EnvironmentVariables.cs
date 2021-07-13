using System;
using System.Linq;


namespace filemon.Variable{

    public class FilemonEnvironmentVariables:FilemonVariable{
        public string[] Handler { get; set; }
        public string Path { get; set; }
        public string ContainerId { get; set; }
        public string OnChangedWebHook { get; set; }
        public string OnRenamedWebHook { get; set; }
        public string OnDeletedWebHook { get; set; }
        public string OnCreatedWebHook { get; set; }

        public FilemonEnvironmentVariables(){
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
            if(string.IsNullOrWhiteSpace(ContainerId))
                throw new ArgumentException("Variable 'CONTAINER_ID' is not set.");

            var WebHookSelected = Handler.Count(x=>x.ToUpper().Trim() == WH)>0;

            OnChangedWebHook = GetVariable("WH_ON_CHANGED");
            OnCreatedWebHook = GetVariable("WH_ON_CREATED");
            OnRenamedWebHook = GetVariable("WH_ON_RENAMED");
            OnDeletedWebHook = GetVariable("WH_ON_DELETED");

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