using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers.V2
{
    //[Authorize(Roles ="Admin")]
    //[ResponseCache(Duration =20)]
    [Route("api/v{version:apiVersion}/Categorias")]
    [ApiController]
    [ApiVersion("2.0")]

    //[EnableCors("PolitcaCors")]
    public class CategoriasV2Controller : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriasV2Controller(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2" };
        }
    }
}
