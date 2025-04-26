using Microsoft.AspNetCore.Mvc;
using ReceitaWS_Project.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("generate-token")]
        public IActionResult GenerateToken([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email é obrigatório para gerar o token.");
            }

            var token = _tokenService.GenerateToken(email);
            return Ok(new { Token = token });
        }
    }

