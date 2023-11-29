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
using Ventas.Services;
using Microsoft.AspNetCore.Authorization;

namespace Ventas.Controllers

{
    [Authorize]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario _context;


        public UsuariosController(IUsuario context)
        {
            _context = context;

        }


        // POST /controller
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public ActionResult Authenticate([FromBody] Usuario usuario)
        {
            var user = _context.Authenticate(usuario.Email, usuario.Password);
            if (user == null)
            {
                return BadRequest(new { mensaje = "Correo o Password Incorrecto." });
            }
            return Ok(user);
        }

        // POST /controller
        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult Register([FromBody] Usuario usuario)
        {
            bool usuarioUnico = _context.UniqueId(usuario.Email);
            if (!usuarioUnico)
            {
                return BadRequest(new { mensaje = "Ya existe un usuario usando la dirección de correo" });
            }

            var registrarse = _context.Register(usuario.Username, usuario.Email, usuario.Password);

            if (registrarse == null)
            {
                return BadRequest(new { mensaje = "Error al registrar el usuario" });
            }

            return Ok(usuario);
        }


        [HttpGet("{id:int}", Name = "GetUsuarioById")]
        public ActionResult<Usuario> GetUsuarioById(int id)
        {
            var user = _context.GetUsuarioById(id);

            if (user == null)
            {
                return NotFound("Usuario no Encontrada.");
            }

            return Ok(user);
        }

        [HttpGet("{Email}", Name = "GetUsuarioByEmail")]
        public ActionResult<Usuario> GetUsuarioByEmail(string Email)
        {
            var user = _context.GetUsuarioByEmail(Email);

            if (user == null)
            {
                return NotFound("No existe el correo.");
            }

            return Ok(user);
        }


    }
}
