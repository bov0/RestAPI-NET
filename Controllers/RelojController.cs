using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RestAPI_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Cors;

namespace RestAPI_NET.Controllers
{
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]
    [ApiController]
    public class RelojController : ControllerBase
    {
        private readonly Iesdawivan02Context _dbcontext;

        public RelojController(Iesdawivan02Context context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Lista()
        {
            try
            {
                List<Relojes> lista = _dbcontext.Relojes.Include(c => c.oMarca).ToList();
                return Ok(new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarPrecio")]
        public IActionResult ListarPrecio()
        {
            try
            {
                List<Relojes> lista = _dbcontext.Relojes.Include(c => c.oMarca).OrderBy(r => r.Precio).ToList();
                return Ok(new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idReloj:int}")]
        public IActionResult Obtener(int idReloj)
        {
            try
            {
                Relojes oReloj = _dbcontext.Relojes.Include(c => c.oMarca).FirstOrDefault(p => p.Id == idReloj);

                if (oReloj == null)
                {
                    return NotFound("Reloj con id " + idReloj + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oReloj });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        [Consumes("multipart/form-data")]
        public IActionResult Guardar([FromForm] RelojFormData formData)
        {
            try
            {
                if (string.IsNullOrEmpty(formData.Modelo))
                {
                    return BadRequest(new { mensaje = "El campo Modelo no puede estar vacío o nulo." });
                }

                if (formData.Precio == null || formData.Precio <= 0)
                {
                    return BadRequest(new { mensaje = "El campo Precio debe tener un valor mayor que 0." });
                }

                if (formData.IdMarca != null && !_dbcontext.Marcas.Any(m => m.Id == formData.IdMarca))
                {
                    return BadRequest(new { mensaje = "El campo IdMarca no existe en la tabla Marcas." });
                }

                if (formData.Imagen != null && formData.Imagen.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(formData.Imagen.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { mensaje = "El formato de la imagen no es válido. Se admiten solo archivos con extensiones .jpg, .jpeg o .png." });
                    }
                }

                Relojes reloj = new Relojes
                {
                    Modelo = formData.Modelo,
                    Precio = formData.Precio,
                    IdMarca = formData.IdMarca,
                    Imagenblob = formData.Imagen.FileName,
                };

                if (formData.Imagen != null && formData.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.Imagen.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        reloj.Imagen = imageBytes;
                    }
                }

                _dbcontext.Relojes.Add(reloj);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = reloj });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        public class RelojFormData
        {
            public string Modelo { get; set; }
            public decimal Precio { get; set; }
            public IFormFile Imagen { get; set; }
            public int? IdMarca { get; set; }
        }

        [HttpPut]
        [Route("EditarReloj/{idReloj:int}")]
        [Consumes("multipart/form-data")]
        public IActionResult EditarReloj(int idReloj, [FromForm] RelojFormData formData)
        {
            try
            {
                if (formData == null)
                {
                    return BadRequest(new { mensaje = "Los datos del reloj no pueden ser nulos." });
                }

                if (idReloj <= 0)
                {
                    return BadRequest(new { mensaje = "El ID del reloj debe ser mayor que cero." });
                }

                Relojes oReloj = _dbcontext.Relojes.Find(idReloj);

                if (oReloj == null)
                {
                    return BadRequest(new { mensaje = "Reloj no encontrado" });
                }

                if (string.IsNullOrEmpty(formData.Modelo))
                {
                    return BadRequest(new { mensaje = "El campo Modelo no puede estar vacío o nulo." });
                }

                if (formData.Precio == null || formData.Precio <= 0)
                {
                    return BadRequest(new { mensaje = "El campo Precio debe tener un valor mayor que 0." });
                }

                if (formData.IdMarca != null && !_dbcontext.Marcas.Any(m => m.Id == formData.IdMarca))
                {
                    return BadRequest(new { mensaje = "El campo IdMarca no existe en la tabla Marcas." });
                }

                if (formData.Imagen != null && formData.Imagen.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(formData.Imagen.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { mensaje = "El formato de la imagen no es válido. Se admiten solo archivos con extensiones .jpg, .jpeg o .png." });
                    }
                }

                oReloj.Modelo = formData.Modelo;
                oReloj.Precio = formData.Precio;
                oReloj.IdMarca = formData.IdMarca;
                oReloj.Imagenblob = formData.Imagen.FileName;

                if (formData.Imagen != null && formData.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.Imagen.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        oReloj.Imagen = imageBytes;
                    }
                }

                _dbcontext.Relojes.Update(oReloj);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = oReloj });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar el reloj", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idReloj:int}")]
        public IActionResult Eliminar(int idReloj)
        {
            try
            {
                Relojes oReloj = _dbcontext.Relojes.Find(idReloj);

                if (oReloj == null)
                {
                    return NotFound("Reloj con id " + idReloj + " no encontrado");
                }

                _dbcontext.Relojes.Remove(oReloj);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ObtenerImagen/{id}")]
        public IActionResult ObtenerImagen(int id)
        {
            try
            {
                Relojes reloj = _dbcontext.Relojes.Find(id);

                if (reloj == null || reloj.Imagen == null)
                {
                    return NotFound("Imagen no encontrada");
                }

                return new FileContentResult(reloj.Imagen, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
