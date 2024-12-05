using Microsoft.Extensions.Logging;
using System.Text.Json;
using App_AEE.Services;
using App_AEE.Model;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;


public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<bool> IsApiDisponivel()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/status");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao verificar disponibilidade da API: {ex.Message}");
            return false;
        }
    }

    // Método de registro
    public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email, string telefone, string password)
    {
        try
        {
            var register = new Register
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Senha = password
            };

            var content = new StringContent(JsonSerializer.Serialize(register, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await PostRequest("https://appaee-a9g2awdggsdmcsc4.brazilsouth-01.azurewebsites.net/api/Usuarios/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro no registro: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    // Método de login
    public async Task<ApiResponse<bool>> Login(string email, string password)
    {
        try
        {
            var login = new Login
            {
                Email = email,
                Senha = password
            };

            // Serializa os dados de login e define o content-type como JSON
            var content = new StringContent(JsonSerializer.Serialize(login, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await PostRequest("https://appaee-a9g2awdggsdmcsc4.brazilsouth-01.azurewebsites.net/api/Usuarios/Login", content);

            // Verifica se a resposta é bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro no login: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            // Desserializa a resposta para o objeto Token
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            if (result == null)
            {
                return new ApiResponse<bool> { ErrorMessage = "Resposta inválida da API." };
            }

            // Armazena os dados importantes do token nas preferências
            Preferences.Set("accesstoken", result.AccessToken);
            Preferences.Set("usuarioid", result.UsuarioId ?? 0);
            Preferences.Set("usuarionome", result.UsuarioNome);
            Preferences.Set("role", result.Role); // Adiciona o papel do usuário (admin/user)

            // Validação opcional do token JWT
            if (!IsTokenValid(result.AccessToken))
            {
                return new ApiResponse<bool> { ErrorMessage = "Token inválido" };
            }

            // Retorna sucesso com base na role
            _logger.LogInformation($"Usuário logado com role: {result.Role}");
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no login: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }


    // Método para verificar se o token JWT é válido
    private bool IsTokenValid(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ZdYM000OLlMQG6VVVp1OH7RxtuEfGvBnXarp7gHuw1qvUC5dcGt3SNM"); // Utilize sua chave secreta ou configure-a no appsettings.json

            // Validação do token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Valida o token
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return true; // Token válido
        }
        catch
        {
            return false; // Token inválido
        }
    }
    public async Task<Usuario> GetUsuarioAtualAsync()
    {
        try
        {
            // Obtendo o token JWT armazenado
            var token = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token JWT não encontrado.");
                return null;
            }

            // Configura o cabeçalho de autorização com o token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Faz a requisição GET para buscar os dados do usuário
            var response = await _httpClient.GetAsync("https://suaapi.com/api/Usuarios/Atual");

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var usuario = JsonSerializer.Deserialize<Usuario>(jsonResult, _serializerOptions);
                return usuario;
            }
            else
            {
                _logger.LogError($"Erro ao obter dados do usuário: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao obter dados do usuário: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> AtualizarUsuarioAsync(EditaUsuario usuarioAtualizado)
    {
        try
        {
            var json = JsonSerializer.Serialize(usuarioAtualizado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://suaapi.com/api/Usuarios/Editar", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
            return false;
        }
    }

    // Enviar requisição POST
    private async Task<HttpResponseMessage> PostRequest(string endpoint, HttpContent content)
    {
        try
        {
            return await _httpClient.PostAsync(endpoint, content);
        }
        catch (HttpRequestException ex)
        {

            _logger.LogError($"Erro na requisição HTTP para {endpoint}: {ex.Message}");
            throw new Exception("Não foi possível conectar ao servidor.");
        }
    }
}
