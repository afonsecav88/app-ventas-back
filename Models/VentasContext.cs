using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ventas.Models
{
    public partial class VentasContext : DbContext
    {
        public VentasContext()
        {
        }

        public VentasContext(DbContextOptions<VentasContext> options)
            : base(options)
        {
        }


        //La propiedad Ventas es la que uso para entity framework y asi mismo se llama la tabla
        //EL DBSet Tarea es el nombre de la clase modelo
        public virtual DbSet<Articulo> Articulos { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

    }
}
