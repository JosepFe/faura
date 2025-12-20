namespace Faura.Infrastructure.Result;

using System.Net;

public class FauraError(string code, string message, HttpStatusCode? httpStatus = null)
{
    public string Code { get; init; } = code;

    public string Message { get; init; } = message;

    public HttpStatusCode HttpStatus { get; private set; } = httpStatus ?? HttpStatusCode.InternalServerError;
}
