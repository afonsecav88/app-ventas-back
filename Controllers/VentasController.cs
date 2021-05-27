using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ventas.DTOs;
using Ventas.Models;
using Ventas.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Ventas.Controllers
{
    [Route("[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentas _context;



        public VentasController(IVentas context)
        {
            _context = context;

        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articulo>>> GetArticulos()
        {
            var articulos = await _context.GetArticulos();

            if (articulos == null)
            {
                return NotFound("No Existen tareas.");
            }

            return Ok(articulos);

        }

    // GET api/<controller>/5
    [HttpGet("{id:int}", Name = "GetArticuloById")]
    public ActionResult<Articulo> GetArticuloById(int id)
    {
        var articulo = _context.GetArticuloById(id);

        if (articulo == null)
        {
            return NotFound("Articulo no Encontrado.");
        }


        return Ok(articulo);
    }


    [HttpGet("{nombre}", Name = "GetArticuloByNombre")]
    public async Task<ActionResult<IEnumerable<Articulo>>> GetArticuloByNombre(string nombre)
    {
        var articulo = await _context.BuscarArticuloNombre(nombre);

        if (nombre == null)
        {
            return NotFound("Debes poner el nombre del Artículo");
        }
        if (articulo == null)
        {
            return NotFound("No exiten Ventas con ese Titulo");
        }

        return Ok(articulo);
    }


    /*
            // POST /controller
            [HttpPost]
            public async Task<ActionResult<Articulo>> CreateArticulo([FromForm]ArticuloVista articulo)
            {
                if (articulo.Imagen != null)
                {
                    var a = _iWebHostEnv.WebRootPath;
                    var fileName = Path.GetFileName(articulo.Imagen.FileName);
                    var filePath = Path.Combine(a , "images\\Articulos", fileName);

                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await articulo.Imagen.CopyToAsync(fileSteam);
                    }

                    Articulo art = new Articulo();
                    art.Nombre = articulo.Nombre;
                    art.Descripcion = articulo.Descripcion;
                    art.Precio = articulo.Precio;
                    art.ImagenPath = filePath;  //save the filePath to database ImagePath field.
                    art.ContactoPropietario = articulo.ContactoPropietario;

                     _context.CreateArticulo(art);
                     _context.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }

*/

    // POST /controller
    [HttpPost]
    public ActionResult<Articulo> CreateArticulo([FromForm]Articulo articulo)
    {
        if (articulo == null)
        {
            return BadRequest(ModelState);
        }

        if (articulo.Imagen.Length > 0)
        {

            _context.CreateArticulo(articulo);
            _context.Save();
            return Ok(" Articulo Creado");

        }
        else
        {
            return BadRequest();

        }

    }


    // PUT /controller>/5
    [HttpPut("{id}", Name = "UpdateArticulo")]
    public ActionResult UpdateArticulo(int Id, [FromForm] Articulo articulo)
    {
        if (articulo.Imagen == null || articulo.Nombre == null || articulo.Descripcion == null || articulo.ContactoPropietario == null)
        {
            return BadRequest(ModelState);
        }


        if (Id != articulo.Id)
        {
            return BadRequest(ModelState);
        }

        if (!_context.BuscarArticuloId(Id))
        {
            return NotFound("No Existe ningun artículo con ese ID.");
        }

        _context.UpdateArticulo(articulo);

        _context.Save();

        return Ok("Artículo actualizado correctamente.");
    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int Id)
    {

        if (!_context.BuscarArticuloId(Id))
        {
            return NotFound("No Existe ningun Artículo con ese ID.");
        }

        var objTarea = _context.GetArticuloById(Id);

        if (!_context.DeleteArticulo(objTarea))
        { 

            return StatusCode(500, ModelState);
        }

        return Ok("Artículo Eliminado Correctamente");


    }


}
}
