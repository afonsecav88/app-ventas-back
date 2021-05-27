using Ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ventas.Interfaces
{
    public interface IUsuario
    {
        bool UniqueId(string email);

        Usuario Authenticate(string email, string password);

        Usuario Register(string username,string email, string password);

        string EncriptarPassword(string password);

        Usuario GetUsuarioById(int UsuarioId);

        Usuario GetUsuarioByEmail(string UsuarioEmail);
               
        //// hecho para password 
        //bool PasswordIsOk(string Password);
    }
}
