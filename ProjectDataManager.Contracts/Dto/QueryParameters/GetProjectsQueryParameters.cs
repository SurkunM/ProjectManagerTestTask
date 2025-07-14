using System.ComponentModel.DataAnnotations;

namespace ProjectDataManager.Contracts.Dto.QueryParameters;

public class GetProjectsQueryParameters
{
    private string _term = string.Empty;

    private string _sortBy = "Name";

    [MaxLength(100)]
    public string Term
    {
        get => _term;
        set => _term = value ?? string.Empty;
    }

    [MaxLength(50)]
    public string SortBy
    {
        get => _sortBy;
        set => _sortBy = value ?? _sortBy;
    }

    public bool IsDescending { get; set; } = false;
}
