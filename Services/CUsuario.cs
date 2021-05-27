 using Ventas.Interfaces;
using Ventas.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Ventas.Services
{
    public class CUsuario : IUsuario
    {
        private readonly VentasContext _usuarios;
        private readonly AppSettings _appSettings;

        public CUsuario(VentasContext  usuarios, IOptions<AppSettings> appSettings)
        {
            _usuarios = usuarios;
            _appSettings = appSettings.Value;
        }
        
        public Usuario Authenticate(string email, string password)
        {
            password = this.EncriptarPassword(password);

            var user = _usuarios.Usuarios.SingleOrDefault(x=>x.Email == email && x.Password == password);

            //usurio no encontrado
            if (user == null)
            {
                return null;
            }

            //Si se encuentra el usurio se genera un token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = " ";
            return user;

        }

 

        public Usuario Register(string username, string email, string password)
        {

            password = this.EncriptarPassword(password);

               Usuario objUsuario = new Usuario
            {
                Username =username,
                Email =email,
                Password = password
            };

            _usuarios.Usuarios.Add(objUsuario);
            _usuarios.SaveChanges();
            objUsuario.Password = "";

            return objUsuario;
        }

        public string EncriptarPassword( string password)
        {
            Byte[] encriptar = Encoding.Unicode.GetBytes(password);
            password = Convert.ToBase64String(encriptar);
            return password;
        }

        public bool UniqueId(string email)
        {
            var user = _usuarios.Usuarios.SingleOrDefault(x => x.Email == email);

            if (user ==null)
            {
                return true;
            }

            return false;
        }

        //// hecho para password 
        //public bool PasswordIsOk(string password)        {

        //    var passwordEncript = this.EncriptarPassword(password);
        //    var user = _usuarios.Usuarios.SingleOrDefault(x => x.Password == passwordEncript);

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}


        public Usuario GetUsuarioById(int UsuarioId)
        {
            return _usuarios.Usuarios.FirstOrDefault(x => x.Id == UsuarioId);
        }

        public Usuario GetUsuarioByEmail(string UsuarioEmail)
        {
            return _usuarios.Usuarios.FirstOrDefault(x => x.Email == UsuarioEmail.ToLower().Trim());
        }
                       
    }
}
