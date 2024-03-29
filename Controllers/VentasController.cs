﻿using System;
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
                return NotFound("No existen árticulos en ventas.");
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


        [HttpGet("findname/{nombre}", Name = "GetArticuloByNombre")]
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

        [HttpGet("findemail/{email}", Name = "BuscarArticulosEmail")]
        public async Task<ActionResult<IEnumerable<Articulo>>> BuscarArticulosEmail(string email)
        {
            var articulos = await _context.BuscarArticulosEmail(email);

            if (email == null)
            {
                return NotFound("Debes escribir algún criterio de búsqueda ");
            }

            if (articulos == null)
            {
                return NotFound("Usted no ha puesto árticulos en venta");
            }

            return Ok(articulos);
        }

        // POST /controller
        [HttpPost]
        public ActionResult<Articulo> CreateArticulo([FromForm] Articulo articulo)
        {
            if (articulo == null)
            {
                return BadRequest(ModelState);
            }

            if (articulo.Imagen.Length > 0)
            {

                _context.CreateArticulo(articulo);
                _context.Save();
                return Ok("Su artículo ha sido insertado correctamente.");

            }
            else
            {
                return BadRequest(new { mensaje = "Su artículo no ha sido insertado " });
            }

        }


        // PUT /controller>/5
        [HttpPut("{id}", Name = "UpdateArticulo")]
        public ActionResult UpdateArticulo(int Id, [FromBody] Articulo articulo)
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
