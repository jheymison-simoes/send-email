namespace SendEmail.Api.ViewModels;

public class BaseResponse<T>
{
    public bool ContainError { get; set; }
    public string MessageError { get; set; }
    public T Response { get; set; }
}