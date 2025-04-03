using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepositorio.GetCategorias();
            var listaCategoriasDTO = _mapper.Map<List<CategoriaDto>>(listaCategorias);
            return Ok(listaCategoriasDTO);
        }

        [HttpGet("{categoriaId:int}", Name="GetCategoria" )]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var categoria = _categoriaRepositorio.GetCategoria(categoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDTO = _mapper.Map<CategoriaDto>(categoria);
            return Ok(categoriaDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
           if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           if( crearCategoriaDto == null)
           {
                return BadRequest(ModelState);
            }

           if (_categoriaRepositorio.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("Nombre", "La categoria ya existe");
                return StatusCode(400, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_categoriaRepositorio.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }
    }
}
