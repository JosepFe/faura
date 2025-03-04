namespace Faura.Infrastructure.Result.Helpers;

using Faura.Infrastructure.Result;

public static class FauraErrorHelper
{
    public static int ToHigherHttpStatusCode(this IEnumerable<FauraError> errors)
        => (int)errors.Max(x => x.HttpStatus);
}
