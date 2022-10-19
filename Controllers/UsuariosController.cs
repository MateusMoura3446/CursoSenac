using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult ListarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View(new UsuarioService().Listar());
        }

        public IActionResult RegistrarUsuarios()
        {
          Autenticacao.CheckLogin(this);
          Autenticacao.verificaSeUsuarioEAdmin(this);
          return View();
        }

       [HttpPost]

        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
          novoUser.Senha = Criptografo.TextoCriptografado(novoUser.Senha);
          new UsuarioService().incluirUsuario(novoUser);
          return View("ListarUsuarios");
        }

        public IActionResult NeedAdmin()
        {
            return View();
        }

        public IActionResult EditarUsuario(int id)
        {
          Autenticacao.CheckLogin(this);
          Autenticacao.verificaSeUsuarioEAdmin(this);
          
          return View(new UsuarioService().buscarPorId(id));
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario userEditado)
        {
          Autenticacao.CheckLogin(this);
          Autenticacao.verificaSeUsuarioEAdmin(this);

          

          new UsuarioService().editarUsuario(userEditado);
          return View("ListarUsuarios");
        }

        public IActionResult ExcluirUsuario(int id)
        {
          Autenticacao.CheckLogin(this);
          Autenticacao.verificaSeUsuarioEAdmin(this);
          new UsuarioService().excluirUsuario(id);
          return View("ListarUsuarios");
        }
    }
}