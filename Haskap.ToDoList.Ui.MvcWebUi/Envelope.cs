namespace Haskap.ToDoList.Ui.MvcWebUi;
public class Envelope<T>
{
    public T? Result { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public DateTime TimeGenerated { get; set; }
    public bool HasError { get; set; }
    public string? ExceptionType { get; set; }

    public Envelope()
    {
    }
}

public class Envelope : Envelope<object>
{

}