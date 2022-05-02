using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Microsoft.Net.Http.Headers;

namespace Haskap.ToDoList.Ui.MvcWebUi.Services;

public class ToDoListService
{
    private readonly HttpClient _httpClient;

    public ToDoListService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri(configuration["ToDoListApiBaseUrl"]);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["jwtToken"]}");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.ContentType, "application/json");
    }

    public async Task<Envelope<IEnumerable<ToDoListOutputDto>>> GetToDoLists() =>
        await _httpClient.GetFromJsonAsync<Envelope<IEnumerable<ToDoListOutputDto>>>("/ToDoList/List");
}
