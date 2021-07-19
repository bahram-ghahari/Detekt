using System;
using System.Collections.Generic;
using System.Linq;
public abstract class FilemonVariable
{
    public event WarningEventHandler WarningCreated;
    protected const string CON="CON";
    protected const string WH="WH";
    protected const string GCP="GCP";
    protected const string AWS="AWS";
    private List<string> ValidHandlers = new List<string>{
        CON // console
        ,WH // web hook
        ,GCP// Google Cloud Storage Bucket
        ,AWS// Amazon's S3
    };
    public string[] Handler { get; set; }
    public string Path { get; set; }
    public string Bucket { get; set; }
    public string ProjectId { get; set; }
    public string ContainerId { get; set; }
    public string OnChangedWebHook { get; set; }
    public string OnRenamedWebHook { get; set; }
    public string OnDeletedWebHook { get; set; }
    public string OnCreatedWebHook { get; set; }
    public string WebHookSignature { get; set; }

    public abstract void Run();


    protected bool IsHandlerValid(string name)=> ValidHandlers.Count(x=>x.ToUpper() == name.Trim().ToUpper())>0;
    
    protected void Warn(string message){
        if(WarningCreated!=null) WarningCreated(new WarningEventArgs(message));

    }

    public FilemonVariable(){
        this.Handler=new string[0];
    }
}
public class WarningEventArgs:EventArgs{
    public string Message { get; set; }
    public WarningEventArgs( string message){
        this.Message = message;
    }
}
public delegate void WarningEventHandler(WarningEventArgs arg);