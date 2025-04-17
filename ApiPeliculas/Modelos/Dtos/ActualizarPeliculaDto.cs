namespace ApiPeliculas.Modelos.Dtos
{
    public class ActualizarPeliculaDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string? RutaImagen { get; set; }

        public string? RutaLocalImagen { get; set; }

        public IFormFile Imagen { get; set; }
        public enum TipoClasificación { siete, Trece, Dieciseis, Dieciocho }

        public TipoClasificación Clasificación { get; set; }

        public DateTime FechaCreación { get; set; }
        public int CategoriaId { get; set; }
    }
}
