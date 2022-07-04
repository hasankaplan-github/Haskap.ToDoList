using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Application.UseCaseServices.Dtos.DataTable;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Haskap.ToDoList.Ui.MvcWebUi.HttpClients;

public class ToDoListHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToDoListHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {httpContextAccessor.HttpContext?.Request.Cookies["jwt"]}");
    }

    #region Account

    public async Task<Envelope<LoginOutputDto>> Login(LoginInputDto input)
    {
        //var httpResponseMessage = await _httpClient.PostAsJsonAsync("/account/login", input);
        var jsonString = JsonSerializer.Serialize(input); //"{\"userName\":\"null\",\"password\":\"null\",\"rememberMe\":true,\"returnUrl\":null}";
        var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        //stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //stringContent.Headers.ContentLength = jsonString.Length;
        //stringContent.Headers.Add("Host", "localhost");

        var httpResponseMessage = await _httpClient.PostAsync("Account/Login", stringContent);
        var envelope = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<LoginOutputDto>>();
        //var result = JsonSerializer.Deserialize<Envelope<LoginOutputDto>>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return envelope!;
    }

    #endregion

    #region ToDoList

    public async Task<Envelope<JqueryDataTableResult>> GetToDoLists(JqueryDataTableParam jqueryDataTableParam)
    {
        //var result = await _httpClient.GetFromJsonAsync<Envelope<IEnumerable<ToDoListOutputDto>>>("/ToDoList/List");
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoList/List", new StringContent(JsonSerializer.Serialize(jqueryDataTableParam), Encoding.UTF8, "application/json"));
        var envelope = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<JqueryDataTableResult>>();

        if (envelope!.HasError ==false)
        {
            envelope.Result!.data = JsonSerializer.Deserialize<IEnumerable<ToDoListOutputDto>>(envelope.Result!.data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return envelope!;
    }

    public async Task<Envelope> AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoList", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
        var envelope = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope>();

        return envelope!;
    }

    public async Task DeleteToDoList(Guid toDoListId)
    {
        var httpResponseMessage = await _httpClient.DeleteAsync($"/ToDoList/{toDoListId}");
    }

    public async Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PutAsync($"/ToDoList/{toDoListId}", new StringContent(JsonSerializer.Serialize(toDoListInputDto)));
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoList/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
    }

    #endregion

    #region ToDoItem

    public async Task AddToDoItem(ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    public async Task DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId)
    {
        var httpResponseMessage = await _httpClient.DeleteAsync($"/ToDoItem/{ownerToDoListId}/{toDoItemId}");
    }

    public async Task UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PutAsync($"/ToDoItem/{toDoItemId}", new StringContent(JsonSerializer.Serialize(toDoItemInputDto)));
    }

    public async Task<Envelope<IEnumerable<ToDoItemOutputDto>>> GetToDoItems(Guid ownerToDoListId)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"/ToDoItem/List/{ownerToDoListId}");
        var envelope = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<IEnumerable<ToDoItemOutputDto>>>();

        return envelope!;
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    public async Task MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem/MarkAsNotCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
    }

    #endregion
}
