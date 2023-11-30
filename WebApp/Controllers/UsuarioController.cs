using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Registrarse()
        {
            // Creamos variable de sesion para controlar visibilidad de un boton particular
            HttpContext.Session.SetString("DondeEstoy", "Registrarse");
            return View();
        }
        [HttpPost]
        public IActionResult Registrarse( 
         string Email, string Contrasenia,
         string Nombre, string Apellido,
         DateTime FechaDeNacimiento)
        {
            Sistema unS = Sistema.Instancia;
            try
            {
                    unS.AgregarUsuario(new Miembro(Nombre, Apellido, 
                        FechaDeNacimiento, new List<Miembro>(),
                        EnumEstadoMiembro.NO_BLOQUEADO, Email, Contrasenia));
                string aviso = $"Fuiste agregado como miembro correctamente. {Email}, ahora a loguearse";
                return RedirectToAction("Login", new { mensaje = aviso });
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }
        public IActionResult Index(string Email) 
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {  
                return Redirect("/Usuario/Login");
            } 
            Sistema unS = Sistema.Instancia;
            Usuario? unU = unS.DevolverUsuario(Email);
            if(HttpContext.Session.GetString("Rol") == "Miembro")
            {
                ViewBag.unM = (Miembro)unU;
            }
            return View(unU);
        }

        public IActionResult Login()
        {
            // Creamos variable de sesion para controlar visibilidad de un boton particular
            HttpContext.Session.SetString("DondeEstoy", "Login");
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Contrasenia)
        {
            Sistema unS = Sistema.Instancia;
            Usuario? unU = unS.DevolverUsuario(Email, Contrasenia);
            if (unU != null)
            {
                HttpContext.Session.SetString("Email", unU.Email); 
                string rol = unU.ObtenerRol();
                HttpContext.Session.SetString("Rol", rol);
                // Creamos variable de sesion para controlar visibilidad de un boton particular
                HttpContext.Session.SetString("DondeEstoy", "Logueado");
                return RedirectToAction("Index", new {email = unU.Email });
            }
            else
            {
                ViewBag.Mensaje = "Credenciales no encontradas";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
    