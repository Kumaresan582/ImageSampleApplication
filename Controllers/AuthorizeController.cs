using Entity.DatabaseConn;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthorizeController(ApplicationDbContext context, IOptions<JwtSettings> options)
        {
            _context = context;
            _jwtSettings = options.Value;
        }

        [HttpPost("generate-token")]
        public async Task<IActionResult> GenerateToken(ViewAuthModel user)
        {
            if (_jwtSettings != null && _context != null && _context.USER_TABLE != null)
            {
                var data = await _context.USER_TABLE.FirstOrDefaultAsync(x => x.USER_NAME == user.USER_NAME && x.PASSWORD == user.PASSWORD);

                if (data != null)
                {
                    var tokengenerate = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.securitykey);
                    var tokendesc = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, data.USER_NAME),
                            new Claim(ClaimTypes.Role, data.Roll)
                        }),
                        Expires = DateTime.UtcNow.AddSeconds(300),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    };

                    var token = tokengenerate.CreateToken(tokendesc);
                    var finaltoken = tokengenerate.WriteToken(token);

                    return Ok(finaltoken);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest("Service configuration error");
            }
        }
    }
}