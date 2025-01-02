
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Application.Comunications;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClearMind.ClearMind.Application.Services
{
    public class GenerateTokenPessoa
    {
        private readonly DBContext _context;

        public GenerateTokenPessoa(DBContext context)
        {
            _context = context;            
        }

        public async Task<AuthenticationResponseJson> GenerateToken(CredenciaisRequestJson credenciaisRequestJson)
        {
            if (credenciaisRequestJson == null)
            {
            throw new ArgumentNullException(nameof(credenciaisRequestJson));
            }

            var JWT_PASS = Environment.GetEnvironmentVariable("JWT_PASS") ?? "d5667v7byn89bv89vb7v7bnv823lky2fdfrfd325";
            var JWT_AUDI = Environment.GetEnvironmentVariable("JWT_AUDI") ?? "ClearMind";
            var JWT_ISSU = Environment.GetEnvironmentVariable("JWT_ISSU") ?? "ClearMind";

            if (_context == null)
            {
                throw new InvalidOperationException("Database context não foi inicializado.");
            }

            var pessoa = await _context.Pessoa.FirstOrDefaultAsync(p => p.Email == credenciaisRequestJson.Email);

            if(pessoa == null || !BCrypt.Net.BCrypt.Verify(credenciaisRequestJson.Senha, pessoa.Senha))
            {
                throw new UnauthorizedAccessException("Nome de usuário ou senha inválidos.");
            }

             // Geração da chave secreta a partir das configurações
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_PASS));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JWT_ISSU,
                audience: JWT_AUDI,
                expires: DateTime.Now.AddHours(1),
                signingCredentials:creds
            );

            return new AuthenticationResponseJson
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = pessoa.Nome
            };
            
        }
       
    }
}