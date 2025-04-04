using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string RutaImagen { get; set; }

        public enum TipoClasificación { siete, Trece, Dieciseis, Dieciocho }

        public TipoClasificación Clasificación {  get; set; }

        public DateTime FechaCreación { get; set; }

        //Relacion con Categoria

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

    }
}
