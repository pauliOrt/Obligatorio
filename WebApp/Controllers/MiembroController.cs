using Dominio;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace WebApp.Controllers
{
    public class MiembroController : Controller
    {
        // Metodo que devuelve objeto tipo Miembro que esta logueado
        public Miembro ObtenerMiembroLogueado()
        {
            Sistema unS = Sistema.Instancia;
            string emailLogueado = HttpContext.Session.GetString("Email");
            Miembro miembroLogueado = (Miembro)unS.DevolverUsuario(emailLogueado);
            return miembroLogueado;
        }
        public IActionResult ListarMiembrosNoAmigos()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro miembroLogueado = ObtenerMiembroLogueado();
            List<Miembro> lm = unS.ListaNoAmigos(miembroLogueado);
            ViewBag.ListaNoAmigos = lm;
            ViewBag.MiembroLogueado = miembroLogueado;
            return View();
        }
        public IActionResult EnviarSolicitud(string Email)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro mSolicitado = (Miembro)unS.DevolverUsuario(Email);
            Miembro mSolicitante = ObtenerMiembroLogueado();
            Invitacion unaI = unS.CrearInvitacion(mSolicitante, mSolicitado);
            unS.AgregarInvitacion(unaI);
            return Redirect("/Miembro/ListarMiembrosNoAmigos");
        }
        public IActionResult ListarInvitacionesPendientes()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro mLogueado = ObtenerMiembroLogueado();
            ViewBag.MiembroLogueado = mLogueado;
            ViewBag.ListaInvitacionesPendientes = unS.ObtenerInvitacionesPendientes(mLogueado.Email);
            return View();
        }
        public IActionResult MiembroAceptarSolicitud(string EmailSolicitante)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro mLogueado = ObtenerMiembroLogueado();
            Invitacion? unaI = unS.DevolverInvitacion(EmailSolicitante, mLogueado.Email);
            unS.AceptarSolicitud(unaI);
            TempData["Mensaje"] = "La solicitud fue aceptada correctamente.";
            return Redirect("/Miembro/ListarInvitacionesPendientes");
        }
        public IActionResult MiembroRechazarSolicitud(string EmailSolicitante)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro mLogueado = ObtenerMiembroLogueado();
            Invitacion? unaI = unS.DevolverInvitacion(EmailSolicitante, mLogueado.Email);
            unS.RechazarSolicitud(unaI);
            TempData["Mensaje"] = "La solicitud fue rechazada correctamente.";
            return Redirect("/Miembro/ListarInvitacionesPendientes");
        }
        public IActionResult MiembroCrearPost()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            ViewBag.MiembroLogueado = ObtenerMiembroLogueado();
            return View();
        }
        [HttpPost]
        public IActionResult MiembroCrearPost(EnumVisibilidad Visibilidad,
            string Titulo, string Contenido, string NombreImagen)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            if (unS.NombreImagenEsValido(NombreImagen))
            {
                unS.CrearPost(NombreImagen, Visibilidad, Titulo, ObtenerMiembroLogueado(), Contenido);
                ViewBag.Mensaje = $"El Post con título \"{Titulo}\" se creó correctamente";
                ViewBag.MiembroLogueado = ObtenerMiembroLogueado();
                return View();
            }
            else
            {
                ViewBag.MensajeError = "El nombre de la imagen no es valido: debe tener extension '.jpg' o '.png'";
                ViewBag.MiembroLogueado = ObtenerMiembroLogueado();
                return View();
            }
        }
        public IActionResult VisualizarPosts()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            ViewBag.ListaPosts = unS.ListarPostsVisiblesMiembro(ObtenerMiembroLogueado());
            ViewBag.MiembroLogueado = ObtenerMiembroLogueado();
            return View();
        }
        public IActionResult MiembroDaLike(int Id)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro miembroLogueado = ObtenerMiembroLogueado();
            Publicacion unaP = unS.DevolverPublicacion(Id);
            unS.MiembroDarLike(miembroLogueado, unaP);
            return Redirect("/Miembro/VisualizarPosts");
        }
        public IActionResult MiembroDaDislike(int Id)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            Miembro miembroLogueado = ObtenerMiembroLogueado();
            Publicacion unaP = unS.DevolverPublicacion(Id);
            unS.MiembroDarDislike(miembroLogueado, unaP);
            return Redirect("/Miembro/VisualizarPosts");
        }
        [HttpPost]
        public IActionResult MiembroCrearComentario(int Id, string Contenido, EnumVisibilidad Visibilidad, string Titulo)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Miembro autor = ObtenerMiembroLogueado();
            if (autor.Estado == EnumEstadoMiembro.BLOQUEADO)
            {
                return Redirect("/Miembro/VisualizarPosts");
            }
            else
            {
                DateTime fecha = DateTime.Now;
                List<Reaccion> reacciones = new List<Reaccion>();
                Comentario comentario = new Comentario(Visibilidad, Titulo, fecha, autor, Contenido, reacciones);
                Sistema unS = Sistema.Instancia;
                Post post = unS.DevolverPost(Id);
                post.Comentarios.Add(comentario);
                return Redirect("/Miembro/VisualizarPosts");
            }
        }
        public IActionResult BuscarContenido()
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult BuscarContenido(int Va, string Texto)
        {
            // Seguridad
            if (HttpContext.Session.GetString("Email") == null)
            {
                return Redirect("/Usuario/Login");
            }
            Sistema unS = Sistema.Instancia;
            List<Publicacion> lp = 
                unS.ListarPublicacionesVisiblesTextoFiltradoVA(Texto, Va, ObtenerMiembroLogueado());
            ViewBag.Lista = lp;
            // Se inicializa una ViewBag para usar como referencia en la vista, de que volvemos con datos desde el método post
            ViewBag.Control = true;
            return View();
        }
    }
}
