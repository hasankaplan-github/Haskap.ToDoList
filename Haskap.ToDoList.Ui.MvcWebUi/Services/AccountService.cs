using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using System.Text.Json;

namespace Haskap.ToDoList.Ui.MvcWebUi.Services;

public class AccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(configuration["ToDoListApiBaseUrl"]);
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["jwtToken"]}");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
        //_httpClient.DefaultRequestHeaders.Add(HeaderNames.ContentType, "application/json");
    }

    public async Task<Envelope<LoginOutputDto>> Login(LoginInputDto input)
    {
        var httpResponseMessage = await _httpClient.PostAsJsonAsync("/account/login", input);
        var resultJson = await httpResponseMessage.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Envelope<LoginOutputDto>>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result;
    }
        
}
