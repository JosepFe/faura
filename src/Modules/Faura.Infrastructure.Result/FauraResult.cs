namespace Faura.Infrastructure.Result;

using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Faura.Infrastructure.Result.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;

public class FauraResult<T> : IFauraResult
{
    public FauraResult(T value) => Data = value;

    protected FauraResult()
    {
    }

    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }

    [JsonIgnore]
    public Type ValueType => typeof(T);

    [JsonInclude]
    public bool IsSuccess => !Errors.Any();

    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<FauraError> Errors { get; protected set; } = [];

    public static implicit operator T(FauraResult<T> result) => result.Data!;

    public static implicit operator FauraResult<T>(T value) => new(value);

    public static FauraResult<T> Success(T value) => new (value);

    public static FauraResult<T> Error(FauraError fauraError) => new()
    {
        Errors = [fauraError],
    };

    public static FauraResult<T> Error(IEnumerable<FauraError>? fauraErrors = null) => new()
    {
        Errors = fauraErrors ?? [],
    };

    public object? GetData() => Data;

    public void AddError(FauraError error)
        => Errors = Errors.Append(error);

    public IEnumerable<FauraError> GetErrors()
        => Errors;

    public bool ReturnedError(FauraError errorCode)
        => Errors?.Any(e => e.Code == errorCode.Code) ?? false;

    public bool ReturnedError(IEnumerable<FauraError> errorCodes)
        => errorCodes.Any(code => Errors?.Any(e => e.Code == code.Code) ?? false);

    public IActionResult BuildResult(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        if (IsSuccess)
        {
            if (EqualityComparer<T>.Default.Equals(Data!, default!))
                return new NoContentResult();

            return new OkObjectResult(Data)
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
