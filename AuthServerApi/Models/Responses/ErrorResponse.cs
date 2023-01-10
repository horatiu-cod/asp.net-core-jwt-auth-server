namespace AuthServerApi.Models.Responses;

public class ErrorResponse
{
    public IEnumerable<string> ErrorMessages { get; set; }

	public ErrorResponse(string errorMesage) : this(new List<string>() { errorMesage }) { }


	public ErrorResponse(IEnumerable<string> errorMessages)
	{
		ErrorMessages = errorMessages;
	}
}
