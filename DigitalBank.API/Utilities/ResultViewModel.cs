namespace DigitalBank.API.Utilities;

public class ResultViewModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }

    public ResultViewModel(bool success, string message, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}