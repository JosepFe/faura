namespace Template.Sample.Domain.Enums;

/// <summary>
/// Status of a sample item.
/// </summary>
public enum SampleStatus
{
    /// <summary>
    /// Item is in draft state.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Item is active and available.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Item is archived.
    /// </summary>
    Archived = 2,
}
