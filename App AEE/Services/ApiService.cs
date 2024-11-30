using App_AEE.Model;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App_AEE.Services
{
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

		// Método para registrar o usuário
		public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email, string telefone, string password)
		{
			try
			{
				var register = new Register()
				{
					Nome = nome,
					Email = email,
					Telefone = telefone,
					Senha = password
				};

				var json = JsonSerializer.Serialize(register, _serializerOptions);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				
				var fullUrl = "http://10.0.2.2:5053/api/Usuarios/Register";

				// Enviando a requisição com a URL completa
				var response = await PostRequest(fullUrl, content);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
					return new ApiResponse<bool>
					{
						ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
					};
				}
				
				
				return new ApiResponse<bool> { Data = true };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
				return new ApiResponse<bool> { ErrorMessage = ex.Message };
			}
		}


		// Método para fazer login
		public async Task<ApiResponse<bool>> Login(string email, string password)
		{
			try
			{
				var login = new Login()
				{
					Email = email,
					Senha = password
				};

				var json = JsonSerializer.Serialize(login, _serializerOptions);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await PostRequest("http://10.0.2.2:5053/api/Usuarios/Login", content);
				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
					return new ApiResponse<bool>
					{
						ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
					};
				}

				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonSerializer.Deserialize<bool>(jsonResult, _serializerOptions);

				return new ApiResponse<bool> { Data = result };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro no login: {ex.Message}");
				return new ApiResponse<bool> { ErrorMessage = ex.Message };
			}
		}

		// Método para fazer upload da foto do usuário
		public async Task<ApiResponse<bool>> UploadImagemUsuario(byte[] imageArray)
		{
			try
			{
				var content = new MultipartFormDataContent();
				content.Add(new ByteArrayContent(imageArray), "imagem", "image.jpg");
				var response = await PostRequest("api/Usuarios/uploadfotousuario", content);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
					return new ApiResponse<bool> { ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}" };
				}

				return new ApiResponse<bool> { Data = true };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao fazer upload da imagem do usuário: {ex.Message}");
				return new ApiResponse<bool> { ErrorMessage = ex.Message };
			}
		}

		// Método para obter a imagem de perfil do usuário
		

		// Método auxiliar para requisição POST
		private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
		{
			var enderecoUrl = AppConfig.BaseUrl + uri;
			try
			{
				var result = await _httpClient.PostAsync(enderecoUrl, content);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao enviar requisição POST para {uri}: {ex.Message}");
				return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
			}
		}

		// Método auxiliar para requisição GET
		private async Task<HttpResponseMessage> GetAsync(string uri)
		{
			var enderecoUrl = AppConfig.BaseUrl + uri;
			try
			{
				var result = await _httpClient.GetAsync(enderecoUrl);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao enviar requisição GET para {uri}: {ex.Message}");
				return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
			}
		}
	}
}