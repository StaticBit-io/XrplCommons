using Microsoft.AspNetCore.Components;
using XrplCommons.Client;
using XrplCommons.Client.Models;

namespace XrplCommons.Blazor.Pages;

public partial class SectionDetail : ComponentBase
{
    [Inject]
    private IXrplCommonsClient Client { get; set; } = default!;

    [Parameter]
    public string Id { get; set; } = string.Empty;

    private Section? _section;
    private List<Category> _categories = [];
    private List<Project> _projects = [];
    private bool _isLoading = true;
    private string _filterQuery = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        try
        {
            Task<Section> sectionTask = Client.GetSectionAsync(Id);
            Task<List<Category>> categoriesTask = Client.GetCategoriesAsync();
            Task<List<Project>> projectsTask = Client.GetProjectsAsync();

            await Task.WhenAll(sectionTask, categoriesTask, projectsTask);

            _section = sectionTask.Result;
            _categories = categoriesTask.Result;
            _projects = projectsTask.Result;
        }
        catch
        {
            _section = null;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private List<Project> GetProjectsForCategory(string categoryId)
    {
        HashSet<string> sectionCategoryIds = _categories
            .Where(c => c.SectionId == Id)
            .Select(c => c.Id)
            .ToHashSet(StringComparer.Ordinal);

        return _projects
            .Where(p =>
                p.Visible &&
                p.CategoryId == categoryId &&
                sectionCategoryIds.Contains(p.CategoryId) &&
                (string.IsNullOrWhiteSpace(_filterQuery) ||
                 p.Name.Contains(_filterQuery, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
