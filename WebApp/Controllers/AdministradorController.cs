using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        // Metodo que devuelve objeto tipo Administrador que esta logueado
        public Administrador ObtenerAdminLogueado()
        {
            Sistema unS = Sistema.Instancia;
            string emailLogueado = HttpContext.Session.GetString("Email");
            Administrador adminLogueado = (Administrador)unS.DevolverUsuario(emailLogueado);
            return adminLogueado;
        }
        public IActionResult ListarMiembros()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            List<Miembro> lm = unS.MostrarMiembros();
            lm.Sort();
            ViewBag.Miembros = lm;
            return View();
        }
        public IActionResult AdminCambiarEstadoMiembro(string Email)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro? unM = unS.DevolverUsuario(Email) as Miembro;
            Administrador unA = ObtenerAdminLogueado();
            unS.CambiarEstadoMiembro(unA, unM);
            return Redirect("/Administrador/ListarMiembros");
        }
        public IActionResult ListarPosts()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            List<Post> lp = unS.MostrarPosts();
            ViewBag.Post = lp;
            return View();
        }
        public IActionResult AdminCambiarEstadoPost(int Id)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Administrador unA = ObtenerAdminLogueado();
            unS.CambiarEstadoPost(unA, Id);
            return Redirect("/Administrador/ListarPosts");
        }
    }
}
