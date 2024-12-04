using Microsoft.Maui.Storage;

public class TokenStorage
{
    public async Task SalvarTokenAsync(string token)
    {
        try
        {
            await SecureStorage.Default.SetAsync("jwtToken", token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar o token: {ex.Message}");
        }
    }

    public async Task<string?> RecuperarTokenAsync()
    {
        try
        {
            return await SecureStorage.Default.GetAsync("jwtToken");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao recuperar o token: {ex.Message}");
            return null;
        }
    }

    public void RemoverToken()
    {
        SecureStorage.Default.Remove("jwtToken");
    }
}