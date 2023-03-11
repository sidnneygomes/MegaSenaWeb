using MegaSenaWeb.Interfaces;
using MegaSenaWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegaSenaWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MegaSenaController : Controller
    {
        private readonly IGeraNumeroMegaSena _geraNumeroMegaSena;

        public MegaSenaController(IGeraNumeroMegaSena geraNumeroMegaSena)
        {
            _geraNumeroMegaSena = geraNumeroMegaSena;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _geraNumeroMegaSena.GetNumeroMegaSena();

            if (result != null) {
                return Ok(result);
            }

            ModelState.AddModelError("ErroConsulta", "Não foi possível gerar os números, tente novamente!");
            return BadRequest(ModelState);
            
        }
    }
}
