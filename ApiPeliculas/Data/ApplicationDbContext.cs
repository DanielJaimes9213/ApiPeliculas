﻿using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Aquí pasar todas las entidades (Modelos)
        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Pelicula> Pelicula { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
