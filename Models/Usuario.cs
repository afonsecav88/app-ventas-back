using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ventas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public List<Articulo> articulos { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }

}
