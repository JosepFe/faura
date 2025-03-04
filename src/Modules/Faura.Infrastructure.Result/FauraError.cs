namespace Faura.Infrastructure.Result;

using System.Net;

public class FauraError
{
    public string Code { get; init; }

    public string Message { get; init; }

    public HttpStatusCode HttpStatus { get; private set; }

    public FauraError(string code, string message, HttpStatusCode? httpStatus = null)
    {
        Code = code;
        Message = message;
        HttpStatus = httpStatus ?? HttpStatusCode.InternalServerError;
    }

}