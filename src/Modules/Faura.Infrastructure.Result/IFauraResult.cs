namespace Faura.Infrastructure.Result;
public interface IFauraResult
{
    IEnumerable<FauraError> Errors { get; }
    Type ValueType { get; }
    object GetData();
}
