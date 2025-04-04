namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearPeliculaDto
    {

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string RutaImagen { get; set; }

        public enum TipoClasificación { siete, Trece, Dieciseis, Dieciocho }

        public TipoClasificación Clasificación { get; set; }
        public int CategoriaId { get; set; }
    }
}
