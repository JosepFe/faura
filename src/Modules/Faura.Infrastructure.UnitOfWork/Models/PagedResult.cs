namespace Faura.Infrastructure.UnitOfWork.Models;

/// <summary>
/// Represents a paginated result set
/// </summary>
public class PagedResult<T> where T : class
{
    /// <summary>
    /// Items in the current page
    /// </summary>
    public IList<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Whether a previous page exists
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Whether a next page exists
    /// </summary>
    public bool HasNextPage { get; set; }
}