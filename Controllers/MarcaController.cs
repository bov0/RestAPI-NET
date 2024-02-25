using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_NET.Models;

namespace RestAPI_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly Iesdawivan02Context _dbcontext;

        public MarcaController(Iesdawivan02Context context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                List<Marcas> lista = _dbcontext.Marcas.ToList();
                return Ok(new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idMarca:int}")]
        public IActionResult Obtener(int idMarca)
        {
            try
            {
                Marcas oMarca = _dbcontext.Marcas.FirstOrDefault(p => p.Id == idMarca);

                if (oMarca == null)
                {
                    return NotFound("Marca con id " + idMarca + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oMarca });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}
