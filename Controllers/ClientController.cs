using APINTUIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace APINTUIT.Controllers
{
    [ApiController]
    [Route("clientes")]
    public class ClientController : ControllerBase
    {
        private readonly DBintuitContext _contexto;
        public ClientController(DBintuitContext contexto)
        {
            _contexto = contexto;
        }


        [HttpGet("all")]
        public ActionResult<IEnumerable<Cliente>> ObtenerTodosLosClientes()
        {
            try
            {
                var clientes = _contexto.Clientes.ToList();
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los clientes: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{id}")]
        public ActionResult<Cliente> ObtenerClientePorId(int id)
        {
            try
            {
                var cliente = _contexto.Clientes.Find(id);

                if (cliente == null)
                {
                    return NotFound();
                }
                return cliente;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar el cliente: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Cliente>> BuscarClientesPorNombre(string nombre)
        {
            try
            {
                var clientes = _contexto.Clientes.Where(c => EF.Functions.Like(c.Nombres, $"%{nombre}%")).ToList();
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar clientes por nombre: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost()]
        public ActionResult<Cliente> AgregarCliente([FromBody] Cliente nuevoCliente)
        {
            try
            {
                // Valida el nuevo cliente
                var validationError = ValidarCliente(nuevoCliente);

                if (validationError != null)
                {
                    return BadRequest(validationError);
                }

                _contexto.Clientes.Add(nuevoCliente);
                _contexto.SaveChanges();

                return CreatedAtAction("ObtenerClientePorId", new { id = nuevoCliente.ID }, nuevoCliente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el cliente: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<Cliente> ActualizarCliente(int id, [FromBody] Cliente clienteActualizado)
        {
            try
            {
                var clienteExistente = _contexto.Clientes.Find(id);

                if (clienteExistente == null)
                {
                    return NotFound("Cliente no encontrado");
                }

                // Actualizar solo los campos proporcionados
                clienteExistente.Nombres = clienteActualizado.Nombres ?? clienteExistente.Nombres;
                clienteExistente.Apellidos = clienteActualizado.Apellidos ?? clienteExistente.Apellidos;
                clienteExistente.FechaNacimiento = clienteActualizado.FechaNacimiento ?? clienteExistente.FechaNacimiento;
                clienteExistente.CUIT = clienteActualizado.CUIT ?? clienteExistente.CUIT;
                clienteExistente.Domicilio = clienteActualizado.Domicilio ?? clienteExistente.Domicilio;
                clienteExistente.Telefono = clienteActualizado.Telefono ?? clienteExistente.Telefono;

                // Validar el cliente actualizado
                var validationError = ValidarCliente(clienteExistente);

                if (validationError != null)
                {
                    return BadRequest(validationError);
                }

                _contexto.SaveChanges();

                return clienteExistente;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        private string ValidarCliente(Cliente cliente)
        {
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(cliente.Nombres) || string.IsNullOrEmpty(cliente.Apellidos) ||
                string.IsNullOrEmpty(cliente.CUIT) || string.IsNullOrEmpty(cliente.Telefono) ||
                string.IsNullOrEmpty(cliente.Correo))
            {
                return "Los campos Nombres, Apellidos, CUIT, Teléfono y Correo son obligatorios.";
            }

            // Validar CUIT
            if (cliente.CUIT.Length != 11 || !cliente.CUIT.All(char.IsDigit))
            {
                return "El CUIT debe tener exactamente 11 caracteres y ser completamente numérico.";
            }

            // Validar formato de Fecha de Nacimiento
            if (cliente.FechaNacimiento.HasValue && !DateTime.TryParseExact(cliente.FechaNacimiento.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return "El formato de la Fecha de Nacimiento debe ser AAAA-MM-DD.";
            }

            // Validar formato de Correo electrónico
            if (!EsEmailValido(cliente.Correo))
            {
                return "El formato del Correo electrónico no es válido.";
            }

            return null; // Sin errores de validación
        }
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    
}
