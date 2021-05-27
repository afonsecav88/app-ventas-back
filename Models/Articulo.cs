using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Ventas.Models
{
    public partial class Articulo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public float Precio { get; set; }
        [Required]
        public string ImagenPath { get; set; }
        [NotMapped]
        public IFormFile Imagen { get; set; }
       
        [Required]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
        public string ContactoPropietario { get; set; }     
    }
}
