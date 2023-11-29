using Ventas.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ventas.Interfaces
{
    public interface IVentas
    {
        Task<IEnumerable<Articulo>> GetArticulos();

        Articulo GetArticuloById(int articuloId);

        Task<IEnumerable<Articulo>> BuscarArticuloNombre(string nombre);

        Task<IEnumerable<Articulo>> BuscarArticulosEmail(string email);

        bool BuscarArticuloId(int articuloId);

        bool CreateArticulo(Articulo articulo);


        void UpdateArticulo(Articulo articulo);


        bool DeleteArticulo(Articulo articulo);

        bool Save();

    }
}
