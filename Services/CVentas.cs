using Microsoft.EntityFrameworkCore;
using Ventas.Models;
using Ventas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Ventas.Services;
	

	

namespace Ventas.Services
{
    public class CVentas : IVentas
    {
        private readonly VentasContext _Ventas;

        private readonly IWebHostEnvironment _iWebHostEnv;



        public CVentas(VentasContext Ventas, IWebHostEnvironment iWebHostEnv )
        {
            _Ventas = Ventas;
            _iWebHostEnv = iWebHostEnv;
           
        }


        public async Task<IEnumerable<Articulo>> GetArticulos()
        {
            return await _Ventas.Articulos.OrderBy(x => x.Nombre).ToListAsync();
        }

            
        
        public bool BuscarArticuloId(int articuloId)
        {
            return _Ventas.Articulos.Any(x => x.Id == articuloId);
        }
      
       


        public bool CreateArticulo(Articulo articulo)
        {
            if (articulo == null)
            {
                throw new ArgumentNullException();
            }

            string path = _iWebHostEnv.WebRootPath + "\\Images\\Articulos\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fileStream = File.Create(path + articulo.Imagen.FileName))
            {
                articulo.Imagen.CopyTo(fileStream);
                fileStream.Flush();
            }

            Articulo art = new Articulo
            {
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Precio = articulo.Precio,
                ImagenPath = path + articulo.Imagen.FileName,
                ContactoPropietario = articulo.ContactoPropietario,
                UsuarioId = articulo.UsuarioId
            };

            _Ventas.Articulos.Add(art);
            return Save();

        }


        public void UpdateArticulo(Articulo articulo)
        {

            string path = _iWebHostEnv.WebRootPath + "\\Images\\Articulos\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fileStream = File.Create(path + articulo.Imagen.FileName))
            {
                articulo.Imagen.CopyTo(fileStream);
                fileStream.Flush();
            }


            Articulo art = this.GetArticuloById(articulo.Id);

            art.Nombre = articulo.Nombre;
            art.Descripcion = articulo.Descripcion;
            art.Precio = articulo.Precio;
            art.ImagenPath = path + articulo.Imagen.FileName;
            art.ContactoPropietario = articulo.ContactoPropietario;

            _Ventas.Articulos.Update(art);
            Save();
        }



        //hacer eliminacion de la BD ,pero no de la imagen fisica
        public bool DeleteArticulo(Articulo articulo)
        {
           _Ventas.Articulos.Remove(articulo);
            return Save();
        }


        public Articulo GetArticuloById(int articuloId)
        {
            return  _Ventas.Articulos.FirstOrDefault(x => x.Id == articuloId);
        }
              
        
        public async Task<IEnumerable<Articulo>> BuscarArticuloNombre(string nombre)
        {
            List<Articulo> listado = await _Ventas.Articulos.Where(x => x.Nombre.Contains(nombre.ToLower().Trim())).ToListAsync();

            return listado;
        }



        public bool Save()
        {
          return _Ventas.SaveChanges()>=0 ? true:false ;
        }
             
              
    }
}
