using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Haskap.ToDoList.Ui.MvcWebUi.Services;

public class ToDoListService
{
    private readonly HttpClient _httpClient;

    public ToDoListService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri(configuration["ToDoListApiBaseUrl"]);
        //_httpClient.DefaultRequestHeaders.Accept.Clear();
        //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["jwt"]}");
//        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.ContentType, "application/json");
    }

    #region Account

    public async Task<Envelope<LoginOutputDto>> Login(LoginInputDto input)
    {
        //var httpResponseMessage = await _httpClient.PostAsJsonAsync("/account/login", input);
        var httpResponseMessage = await _httpClient.PostAsync("/account/login", new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json"));
        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<LoginOutputDto>>();
        //var result = JsonSerializer.Deserialize<Envelope<LoginOutputDto>>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result;
    }

    #endregion

    #region ToDoList

    public async Task<Envelope<IEnumerable<ToDoListOutputDto>>> GetToDoLists()
    {
        //var result = await _httpClient.GetFromJsonAsync<Envelope<IEnumerable<ToDoListOutputDto>>>("/ToDoList/List");
        var httpResponseMessage = await _httpClient.GetAsync("/ToDoList/List");
        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<IEnumerable<ToDoListOutputDto>>>();

        return result;
    }

    public async Task AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        await _httpClient.PostAsync("/ToDoList", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
    }

    public async Task DeleteToDoList(Guid toDoListId)
    {
        await _httpClient.DeleteAsync($"/ToDoList/{toDoListId}");
    }

    public async Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        await _httpClient.PutAsync($"/ToDoList/{toDoListId}", new StringContent(JsonSerializer.Serialize(toDoListInputDto)));
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListInputDto)
    {
        await _httpClient.PostAsync("/ToDoList/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
    }

    #endregion

    #region ToDoItem

    public async Task AddToDoItem(ToDoItemInputDto toDoItemInputDto)
    {
        await _httpClient.PostAsync("/ToDoItem", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    public async Task DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId)
    {
        await _httpClient.DeleteAsync($"/ToDoItem/{ownerToDoListId}/{toDoItemId}");
    }

    public async Task UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        await _httpClient.PutAsync($"/ToDoItem/{toDoItemId}", new StringContent(JsonSerializer.Serialize(toDoItemInputDto)));
    }

    public async Task<Envelope<IEnumerable<ToDoItemOutputDto>>> GetToDoItems(Guid ownerToDoListId)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"/ToDoItem/List/{ownerToDoListId}");
        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<IEnumerable<ToDoItemOutputDto>>>();

        return result;
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _httpClient.PostAsync("/ToDoItem/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    public async Task MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        await _httpClient.PostAsync("/ToDoItem/MarkAsNotCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    #endregion
}
