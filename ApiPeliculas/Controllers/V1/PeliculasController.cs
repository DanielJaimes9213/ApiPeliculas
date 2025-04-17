using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers.V1
{
    [Route("api/v{version:apiVersion}/Peliculas")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _peliculaRepositorio;
        private readonly IMapper _mapper;
        public PeliculasController(IPeliculaRepositorio peliculaRepositorio, IMapper mapper)
        {
            _peliculaRepositorio = peliculaRepositorio;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculaRepositorio.GetPeliculas();
            var listaPeliculasDto = _mapper.Map<List<PeliculaDto>>(listaPeliculas);
            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var pelicula = _peliculaRepositorio.GetPelicula(peliculaId);
            if (pelicula == null)
            {
                return NotFound();
            }
            var PeliculaDTO = _mapper.Map<PeliculaDto>(pelicula);
            return Ok(PeliculaDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPelicula([FromForm] CrearPeliculaDto crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_peliculaRepositorio.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("Nombre", "La película ya existe");
                return StatusCode(400, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

            //if (!_peliculaRepositorio.CrearPelicula(pelicula))
            //{
            //    ModelState.AddModelError("", $"Algo salio mal guardando el registro {pelicula.Nombre}");
            //    return StatusCode(500, ModelState);
            //}

            //Subida de Archivo
            if (crearPeliculaDto.Imagen != null)
            {
                string nombreArchivo = pelicula.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(crearPeliculaDto.Imagen.FileName);
                string rutaArchivo = @"wwwroot\ImagenesPeliculas\" + nombreArchivo;

                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

                FileInfo file = new FileInfo(ubicacionDirectorio);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    crearPeliculaDto.Imagen.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaImagen = baseUrl + "/ImagenesPeliculas/" + nombreArchivo;
                pelicula.RutaLocalImagen = rutaArchivo;
            }
            else

            {
                pelicula.RutaImagen = "https://placehold.co/600x400";
            }

            _peliculaRepositorio.CrearPelicula(pelicula);
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromForm] ActualizarPeliculaDto actualizarPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarPeliculaDto == null || peliculaId != actualizarPeliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var peliculaExistente = _peliculaRepositorio.GetPelicula(peliculaId);

            if (peliculaExistente == null)
            {
                return NotFound($"No se encontra la película con ID: {peliculaId}");
            }

            var pelicula = _mapper.Map<Pelicula>(actualizarPeliculaDto);

            //if (!_peliculaRepositorio.ActualizarPelicula(pelicula))
            //{
            //    ModelState.AddModelError("", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
            //    return StatusCode(500, ModelState);
            //}

            //Subida de Archivo
            if (actualizarPeliculaDto.Imagen != null)
            {
                string nombreArchivo = pelicula.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(actualizarPeliculaDto.Imagen.FileName);
                string rutaArchivo = @"wwwroot\ImagenesPeliculas\" + nombreArchivo;

                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

                FileInfo file = new FileInfo(ubicacionDirectorio);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    actualizarPeliculaDto.Imagen.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaImagen = baseUrl + "/ImagenesPeliculas/" + nombreArchivo;
                pelicula.RutaLocalImagen = rutaArchivo;
            }
            else

            {
                pelicula.RutaImagen = "https://placehold.co/600x400";
            }

            _peliculaRepositorio.ActualizarPelicula(pelicula);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{peliculaId:int}", Name = "EliminarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EliminarPelicula(int peliculaId)
        {
            if (!_peliculaRepositorio.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _peliculaRepositorio.GetPelicula(peliculaId);

            if (!_peliculaRepositorio.EliminarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            try
            {
                var listaPeliculas = _peliculaRepositorio.GetPeliculasEnCategoria(categoriaId);

                if (listaPeliculas == null || !listaPeliculas.Any())
                {
                    return NotFound($"No se encontaron películas en la categoria con ID {categoriaId}");
                }

                var listasPeliculasDto = _mapper.Map<List<PeliculaDto>>(listaPeliculas);
                return Ok(listasPeliculasDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }

        [AllowAnonymous]
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _peliculaRepositorio.BuscarPelicula(nombre);

                if (resultado.Any())
                {
                    var resultadoDto = _mapper.Map<List<PeliculaDto>>(resultado);
                    return Ok(resultadoDto);
                }

                return NotFound($"No se encontraron resultados para la búsqueda: {nombre}");

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
    }
}
