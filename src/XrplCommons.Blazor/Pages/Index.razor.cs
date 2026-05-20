using Microsoft.AspNetCore.Components;
using XrplCommons.Client;
using XrplCommons.Client.Models;

namespace XrplCommons.Blazor.Pages;

public partial class Index : ComponentBase
{
    [Inject]
    private IXrplCommonsClient Client { get; set; } = default!;

    private List<Section> _sections = [];
    private List<Category> _categories = [];
    private List<Project> _projects = [];
    private bool _isLoading = true;
    private string? _error;
    private string _searchQuery = string.Empty;

    private IEnumerable<Project> FilteredProjects =>
        _projects.Where(p =>
            p.Visible &&
            (p.Name.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
             p.Description.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
             p.Status.ToString().Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
             (p.Tags?.Any(t => t.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)) ?? false)));

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _isLoading = true;
        _error = null;

        try
        {
            Task<List<Section>> sectionsTask = Client.GetSectionsAsync();
            Task<List<Category>> categoriesTask = Client.GetCategoriesAsync();
            Task<List<Project>> projectsTask = Client.GetProjectsAsync();

            await Task.WhenAll(sectionsTask, categoriesTask, projectsTask);

            _sections = sectionsTask.Result;
            _categories = categoriesTask.Result;
            _projects = projectsTask.Result;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private int GetCategoryCount(string sectionId) =>
        _categories.Count(c => c.SectionId == sectionId);
}
