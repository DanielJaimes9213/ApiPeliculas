﻿using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            //Arreglar el problema del put
            var categoriaExistente = _db.Categoria.Find(categoria.Id);
            if (categoriaExistente != null)
            {
                _db.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }

            else
            {
                _db.Categoria.Update(categoria);
            }

            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categoria.Add(categoria);
            return Guardar();
        }

        public bool EliminarCategoria(Categoria categoria)
        {
           _db.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(int id)
        {
            return _db.Categoria.Any(c => c.Id == id);
        }

        public bool ExisteCategoria(string nombre)
        {
           bool valor = _db.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
           return valor;
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
           return _db.Categoria.OrderBy(c => c.Id).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
