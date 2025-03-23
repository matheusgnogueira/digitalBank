namespace DigitalBank.Tests.Helpers;

public class ResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
