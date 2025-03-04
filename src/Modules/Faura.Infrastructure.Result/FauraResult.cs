namespace Faura.Infrastructure.Result;

using Faura.Infrastructure.Result.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;

public class FauraResult<T> : IFauraResult
{
    protected FauraResult() { }

    public FauraResult(T value)
    {
        Data = value;
    }

    public static implicit operator T(FauraResult<T> result) => result.Data;
    public static implicit operator FauraResult<T>(T value) => new FauraResult<T>(value);

    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data { get; init; }

    [JsonIgnore]
    public Type ValueType => typeof(T);

    [JsonInclude]
    public bool IsSuccess => !Errors.Any();

    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<FauraError> Errors { get; protected set; } = [];

    public object GetData()
    {
        return Data;
    }

    public static FauraResult<T> Success(T value)
    {
        return new FauraResult<T>(value);
    }

    public static FauraResult<T> Error(FauraError fauraError)
    {
        return new FauraResult<T>() { Errors = [fauraError] };
    }

    public static FauraResult<T> Error(IEnumerable<FauraError> fauraErrors = null)
    {
        return new FauraResult<T>()
        {
            Errors = fauraErrors
        };
    }

    public void AddError(FauraError error)
    {
        IEnumerable<FauraError> errors;
        if (Errors != null)
        {
            errors = Errors.Append(error);
        }
        else
        {
            errors = [];
        }

        Errors = errors;
    }

    public IEnumerable<FauraError> GetErrors()
    {
        return Errors;
    }

    public bool ReturnedError(FauraError errorCode)
    {
        return Errors?.Any((myError) => myError.Code == $"{errorCode}") ?? false;
    }

    public bool ReturnedError(IEnumerable<FauraError> errorCodes)
    {
        return errorCodes.Any((errorCode) => Errors?.Any((myError) => myError.Code == $"{errorCode}") ?? false);
    }

    public IActionResult BuildResult(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        if (IsSuccess)
        {
            T dataAsT = Data;
            if (dataAsT == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(dataAsT)
            {
                StatusCode = (int)httpStatusCode,
            };

        }
        return new ObjectResult(Errors)
        {
            StatusCode = Errors.ToHigherHttpStatusCode(),
        };
    }
}