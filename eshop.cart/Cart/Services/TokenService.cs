using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TokenOptions = Cart.Configurations.TokenOptions;

namespace Cart.Services;

public class TokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _securityKey;
    private readonly string _keyId;

    public TokenService(IHttpContextAccessor httpContextAccessor, IOptions<TokenOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _securityKey = options.Value.SecurityKey;
        _keyId = options.Value.KeyId;
    }

    public string GetEmail()
    {
        var jwtToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "").Trim();
        
        var eccKey = ECDsa.Create();
        eccKey.ImportSubjectPublicKeyInfo(Convert.FromBase64String(_securityKey), out _);

        var securityKey = new ECDsaSecurityKey(eccKey)
        {
            KeyId = _keyId
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
        };

        var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out _);

        var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        return emailClaim;
    }
}