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
    string[] Handler { get; set; }
    string Path { get; set; }
    string ContainerId { get; set; }
    string OnChangedWebHook { get; set; }
    string OnRenamedWebHook { get; set; }
    string OnDeletedWebHook { get; set; }
    string OnCreatedWebHook { get; set; }

    protected bool IsHandlerValid(string name)=> ValidHandlers.Count(x=>x.ToUpper() == name.Trim().ToUpper())>0;
    
    protected void Warn(string message){
        if(WarningCreated!=null) WarningCreated(new WarningEventArgs(message));

    }
}
public class WarningEventArgs:EventArgs{
    public string Message { get; set; }
    public WarningEventArgs( string message){
        this.Message = message;
    }
}
public delegate EventHandler WarningEventHandler(WarningEventArgs arg);