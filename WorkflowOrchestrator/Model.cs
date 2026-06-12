public class Workflow
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Status {get; set;} = "Pending";
    public List<WorkflowStep> Steps {get; set;} = new List<WorkflowStep>();
}

public class WorkflowStep
{
    public int Id {get; set;}
    public int WorkflowId {get; set;}
    public string Description {get; set;} = string.Empty;
    public int OrderIndex {get; set;}
    public bool IsCompleted {get; set;} = false;
    
}