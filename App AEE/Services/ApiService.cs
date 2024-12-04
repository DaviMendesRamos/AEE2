﻿using Microsoft.Extensions.Logging;
using System.Text.Json;
using App_AEE.Services;
using App_AEE.Model;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


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

            var content = new StringContent(JsonSerializer.Serialize(login, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await PostRequest("https://appaee-a9g2awdggsdmcsc4.brazilsouth-01.azurewebsites.net/api/Usuarios/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro no login: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            // Armazena o token JWT nas preferências
            Preferences.Set("accesstoken", result!.AccessToken);
            Preferences.Set("usuarioid", result.UsuarioId ?? 0);
            Preferences.Set("usuarionome", result.UsuarioNome);

            // Valida o JWT (Opcional, você pode usar esta validação em todas as requisições)
            if (!IsTokenValid(result.AccessToken))
            {
                return new ApiResponse<bool> { ErrorMessage = "Token inválido" };
            }

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