using System.ComponentModel.DataAnnotations;

namespace APINTUIT.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? CUIT { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

    }
}
