namespace Webworks.DTOs
{
    public class ErrorInfo
    {

        public ErrorInfo(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}