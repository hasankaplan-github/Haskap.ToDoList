using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Microsoft.Net.Http.Headers;
using System.Net;
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

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["jwt"]}");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.ContentLength, "");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.Host, "localhost");
        
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
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<IEnumerable<ToDoListOutputDto>>>();

        return result;
    }

    public async Task<Envelope<object>> AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoList", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        //httpResponseMessage.EnsureSuccessStatusCode();
        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<object>>();

        return result;
    }

    public async Task DeleteToDoList(Guid toDoListId)
    {
        var httpResponseMessage = await _httpClient.DeleteAsync($"/ToDoList/{toDoListId}");
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PutAsync($"/ToDoList/{toDoListId}", new StringContent(JsonSerializer.Serialize(toDoListInputDto)));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoList/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoListInputDto), Encoding.UTF8, "application/json"));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    #endregion

    #region ToDoItem

    public async Task AddToDoItem(ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId)
    {
        var httpResponseMessage = await _httpClient.DeleteAsync($"/ToDoItem/{ownerToDoListId}/{toDoItemId}");
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PutAsync($"/ToDoItem/{toDoItemId}", new StringContent(JsonSerializer.Serialize(toDoItemInputDto)));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task<Envelope<IEnumerable<ToDoItemOutputDto>>> GetToDoItems(Guid ownerToDoListId)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"/ToDoItem/List/{ownerToDoListId}");
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
        var result = await httpResponseMessage.Content.ReadFromJsonAsync<Envelope<IEnumerable<ToDoItemOutputDto>>>();

        return result;
    }

    public async Task MarkAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem/MarkAsCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public async Task MarkAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var httpResponseMessage = await _httpClient.PostAsync("/ToDoItem/MarkAsNotCompleted", new StringContent(JsonSerializer.Serialize(toDoItemInputDto), Encoding.UTF8, "application/json"));
        if (httpResponseMessage.IsSuccessStatusCode == false && httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }

    #endregion
}
