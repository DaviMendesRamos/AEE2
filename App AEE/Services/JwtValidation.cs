using System.IdentityModel.Tokens.Jwt;

public class JwtValidationService
{
    public bool IsTokenValido(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(token))
            return false;

        var jwtToken = jwtHandler.ReadJwtToken(token);

        // Verifica se o token expirou
        return jwtToken.ValidTo > DateTime.UtcNow;
    }
}