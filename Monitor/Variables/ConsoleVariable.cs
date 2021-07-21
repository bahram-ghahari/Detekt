using System;
using System.Linq;


namespace filemon.Variable{

    public class ConsoleVariable:FilemonVariable{
 

        public ConsoleVariable(string[] args){
        }

        public override void Run(){



            var _handler = GetVariable("DETEKT_CHANGE_HANDLER");
            var _handlers = new string[0];
            while(string.IsNullOrEmpty(_handler)){
                _handler = Ask(
                    "Which change handlers would you like to use? use | to separate handlers"+Environment.NewLine+Environment.NewLine+
                    "[WH]  web hook"+Environment.NewLine+
                    "[GCP] Google cloud Platform storage"+Environment.NewLine+
                    "[AWS] Amazon s3 bucket"+Environment.NewLine+
                    "[CON] Console output"+Environment.NewLine
                    );
                _handlers = _handler.ToUpper().Split('|',StringSplitOptions.RemoveEmptyEntries);
                if(_handlers.Length==0)
                {
                    _handler  = string.Empty;
                    Warn("Entered value is invalid!");
                    continue;
                }
                var all_handlers_are_valid = true;
                for (int i = 0; i < _handlers.Length; i++){
                    if(!IsHandlerValid(_handlers[i] )){
                        all_handlers_are_valid=false;
                        Warn(_handlers[i]+" is not a valid DETEKT handler.");
                    }
                }
                if(!all_handlers_are_valid)
                {
                    _handler=string.Empty;
                    continue;
                }  
            }
            this.Handler = _handler.ToUpper().Split('|',StringSplitOptions.RemoveEmptyEntries);
            
            var _path = GetVariable("DETEKT_PATH_TO_WATCH");
            while(string.IsNullOrEmpty(_path))
            {
                _path = Ask("Enter a path to watch:");
                _path = _path.Trim().Replace(Environment.NewLine,"");
                if(string.IsNullOrEmpty( _path ))
                {
                    Warn("Invalid path!"); 
                    continue;
                }
                var dir = new System.IO.DirectoryInfo(_path);
                if(!dir.Exists){
                    Warn("Invalid path!");
                    _path = string.Empty;
                }  
            }
            Path = _path;
                


            var _container_id = GetVariable("DETEKT_CONTAINER_ID");
            if(string.IsNullOrWhiteSpace(_container_id)){

                _container_id = Ask("Would you like to add a container name? If not, an automatic id will be generated");
                if(string.IsNullOrWhiteSpace(_container_id)){
                    _container_id = "con-";
                    var rand = new Random();
                    var random_number = rand.Next(1000000,9999999);
                    _container_id+=random_number;
                    Warn(_container_id+"' was generated randomly as the container name.");
                }
            }
            this.ContainerId = _container_id;




            var WebHookSelected = Handler.Count(x=>x.ToUpper().Trim() == WH)>0;
            var GcpSelected = Handler.Count(x=>x.ToUpper().Trim() == GCP)>0;


            var _bucket  = GetVariable("DETEKT_GCP_BUCKET");
            var _project_id = GetVariable("DETEKT_GCP_PROJECT_ID");

            var _onChangedWebHook = GetVariable("DETEKT_WH_ON_CHANGED");
            var _onCreatedWebHook = GetVariable("DETEKT_WH_ON_CREATED");
            var _onRenamedWebHook = GetVariable("DETEKT_WH_ON_RENAMED");
            var _onDeletedWebHook = GetVariable("DETEKT_WH_ON_DELETED");
            var _webHookSignature = GetVariable("DETEKT_WH_SIGNATURE");




            if(GcpSelected){
                if(string.IsNullOrWhiteSpace(_bucket)){
                    _bucket = Ask("What's Google storage bucket name? If you leave it empty, it'll be the same as the container name.");
                    if(string.IsNullOrWhiteSpace(_bucket)) _bucket = ContainerId; 
                    Warn("Bucket name is set to  '"+ContainerId ); 
                }
                Bucket = _bucket;
 
                while(string.IsNullOrWhiteSpace(_project_id)){
                    _project_id = Ask("Enter GCP Project Id."); 
                    if(string.IsNullOrEmpty( _project_id ))
                    {
                        Warn("Please add a valid project id!"); 
                        continue;
                    }
                }
                ProjectId = _project_id;   

                var _credentials = GetVariable("GOOGLE_APPLICATION_CREDENTIALS");
                if(string.IsNullOrWhiteSpace(_credentials)){
                    throw new ArgumentException("Environment variable 'GOOGLE_APPLICATION_CREDENTIALS' is not set. ");
                }
            }



            if(WebHookSelected){
                if(string.IsNullOrWhiteSpace( _onChangedWebHook )){
                    _onChangedWebHook = Ask("What web hook would you like to use for changes?");
                    if(string.IsNullOrWhiteSpace(_onChangedWebHook))
                        Warn("File changes will not be captured."); 
                }
                OnChangedWebHook = _onChangedWebHook;

                if(string.IsNullOrWhiteSpace( _onDeletedWebHook )){
                    _onDeletedWebHook = Ask("What web hook would you like to use for deletions?");
                    if(string.IsNullOrWhiteSpace(_onDeletedWebHook))
                        Warn("File deletions will not be captured."); 
                }
                OnDeletedWebHook = _onDeletedWebHook;

                if(string.IsNullOrWhiteSpace( _onRenamedWebHook )){
                    _onRenamedWebHook = Ask("What web hook would you like to use for renaming?");
                    if(string.IsNullOrWhiteSpace(_onRenamedWebHook))
                        Warn("File name changes will not be captured."); 
                }
                OnRenamedWebHook = _onRenamedWebHook; 

                if(string.IsNullOrWhiteSpace( _onCreatedWebHook )){
                    _onCreatedWebHook = Ask("What web hook would you like to use for new files?");
                    if(string.IsNullOrWhiteSpace(_onCreatedWebHook))
                        Warn("New files will not be captured."); 
                }
                OnCreatedWebHook = _onCreatedWebHook;

            }
        }

        private string GetVariable(string name){
            return Environment.GetEnvironmentVariable(name);
        }
    }

}