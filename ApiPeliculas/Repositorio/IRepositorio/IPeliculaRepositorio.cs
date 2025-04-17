﻿using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio 
    {
        //ICollection<Pelicula> GetPeliculas();

        ICollection<Pelicula> GetPeliculas(int pageNumer, int pageSize);

        int GetTotalPeliculas();

        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);

        IEnumerable<Pelicula> BuscarPelicula(string nombre);
        Pelicula GetPelicula(int peliculaId);

        bool ExistePelicula(int id);

        bool ExistePelicula(string nombre);

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool EliminarPelicula(Pelicula pelicula);

        bool Guardar();
    }
}
