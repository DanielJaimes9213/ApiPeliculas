namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioLoginRespuesta
    {
        public UsuarioDatosDto Usuario { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }
    }
}
