namespace Faura.Infrastructure.UnitOfWork.Models;

/// <summary>
/// Represents a paginated result set.
/// </summary>
public class PagedResult<T>
    where T : class
{
    /// <summary>
    /// Gets or sets items in the current page.
    /// </summary>
    public IList<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets total number of items across all pages.
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// Gets or sets total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether whether a previous page exists.
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether whether a next page exists.
    /// </summary>
    public bool HasNextPage { get; set; }
}
