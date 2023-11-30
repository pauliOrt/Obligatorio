using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class Sistema
    {
        #region Atributos
        private static Sistema instancia;
        private List<Invitacion> _invitaciones = new List<Invitacion>();
        private List<Publicacion> _publicaciones = new List<Publicacion>();
        private List<Usuario> _usuarios = new List<Usuario>();
        #endregion
        #region Singleton
        public static Sistema Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Sistema();
                }
                return instancia;
            }
        }
        private Sistema()
        {
            Precarga();
        }
        #endregion
        #region Propiedades
        #endregion
        #region Métodos
        public List<Usuario> Usuarios()
        {
            return _usuarios;
        }
        public List<Publicacion> Publicaciones()
        {
            return _publicaciones;
        }
        public List<Invitacion> Invitaciones()
        {
            return _invitaciones;
        }
        public bool AgregarPublicacion(Publicacion publicacion)
        {
            bool resultado = false;
            if (publicacion != null)
            {
                _publicaciones.Add(publicacion);
                resultado = true;
            }
            return resultado;
        }
        public bool AgregarInvitacion(Invitacion invitacion)
        {
            bool resultado = false;
            if (invitacion != null)
            {
                _invitaciones.Add(invitacion);
                resultado = true;
            }
            return resultado;
        }
        public void AgregarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    throw new Exception("Usuario nulo");
                }
                usuario.Validar();
                this.VerificarUsuarioNoExiste(usuario);
                _usuarios.Add(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void VerificarUsuarioNoExiste(Usuario usuario)
        {
            if (Usuarios().Contains(usuario))
            {
                throw new Exception("Usuario ya existe");
            }
        }
        public void AgregarComentario(Comentario comentario, Post post)
        {
            post.Comentarios.Add(comentario);
        }
        public List<Publicacion> ListarPublicaciones(string email)
        {
            List<Publicacion> auxiliar = new List<Publicacion>();
            foreach (Publicacion p in Publicaciones())
            {
                if (p.Autor.Email == email)
                {
                    auxiliar.Add(p);
                }
                if (p is Post post)
                {
                    foreach (Comentario c in post.Comentarios)
                    {
                        if (c.Autor.Email == email)
                        { auxiliar.Add(c); }
                    }
                }
            }
            return auxiliar;
        }
        public List<Post> ListarPostPorFecha(DateTime fecha1, DateTime fecha2)
        {
            List<Post> auxiliar = new List<Post>();
            foreach (Publicacion p in Publicaciones())
            {
                if (p is Post && p.Fecha >= fecha1 && p.Fecha <= fecha2)
                {
                    Post temporal = (Post)p;
                    auxiliar.Add(temporal);
                }
            }
            auxiliar.Sort();
            return auxiliar;
        }
        public List<Post> ListarPostComentariosEmail(string email)
        {
            List<Post> auxiliar = new List<Post>();
            foreach (Publicacion p in Publicaciones())
            {
                if (p is Post)
                {
                    Post ps = (Post)p;
                    bool comentarioEmail = false;
                    foreach (Comentario c in ps.Comentarios)
                    {
                        if (c.Autor.Email == email)
                        {
                            comentarioEmail = true;
                            break;
                        }
                    }
                    if (comentarioEmail) auxiliar.Add(ps);
                }
            }
            return auxiliar;
        }
        public List<Usuario> ListarTopMiembros()
        {
            List<Usuario> auxiliarU = new List<Usuario>();
            int maximo = 0;
            foreach (Usuario usuario in Usuarios())
            {
                string email = usuario.Email;
                List<Publicacion> lista = ListarPublicaciones(email);
                if (lista.Count > maximo)
                {
                    auxiliarU.Clear();
                    maximo = lista.Count;
                }
                if (lista.Count == maximo)
                {
                    auxiliarU.Add(usuario);
                }
            }
            return auxiliarU;
        }
        public Usuario? DevolverUsuario(string email)
        {
            foreach (Usuario usuario in Usuarios())
            {
                if (usuario.Email == email) return usuario;
            }
            return null;
        }
        public Usuario? DevolverUsuario(string email, string contrasenia)
        {
            foreach (Usuario usuario in Usuarios())
            {
                if (usuario.Email == email && usuario.Contrasenia == contrasenia) return usuario;
            }
            return null;
        }
        public List<Miembro> MostrarMiembros()
        {
            List<Miembro> lista = new List<Miembro>();
            foreach (Usuario unU in Usuarios())
            {
                if (unU is Miembro)
                {
                    Miembro unM = (Miembro)unU;
                    lista.Add(unM);
                }
            }
            return lista;
        }
        public List<Post> MostrarPosts()
        {
            List<Post> posts = new List<Post>();
            foreach (Publicacion unaP in Publicaciones())
            {
                if (unaP is Post)
                {
                    Post p = (Post)unaP;
                    posts.Add(p);
                }
            }
            return posts;
        }
        public Post? DevolverPost(int id)
        {
            foreach (Post unP in MostrarPosts())
            {
                if (unP.Id == id) return unP;
            }
            return null;
        }
        public List<Miembro> ListaNoAmigos(Miembro unM)
        {
            List<Miembro> aux = new List<Miembro>();
            foreach (Miembro miembro in MostrarMiembros())
            {
                if (unM.EsAmigo(miembro) == false)
                {
                    if (miembro.Email != unM.Email)
                        aux.Add(miembro);
                }
            }
            return aux;
        }
        public bool ExisteSolicitud(string emailSolicitante, string emailSolicitado)
        {
            bool resultado = false;
            foreach (Invitacion i in Invitaciones())
            {
                if (i.Solicitante.Email == emailSolicitante && i.Solicitado.Email == emailSolicitado)
                {
                    return true;
                }
            }
            return resultado;
        }
        public Invitacion? DevolverInvitacion(string emailSolicitante, string emailSolicitado)
        {
            foreach (Invitacion invitacion in Invitaciones())
            {
                if (invitacion.Solicitante.Email == emailSolicitante && invitacion.Solicitado.Email == emailSolicitado)
                {
                    return invitacion;
                }
            }
            return null;
        }
        public List<Invitacion> ObtenerInvitacionesPendientes(string email)
        {
            List<Invitacion> li = new List<Invitacion>();
            foreach (Invitacion i in Invitaciones())
            {
                if (email == i.Solicitado.Email)
                {
                    if (i.EstadoInvitacion == EnumEstadoInvitacion.PENDIENTE_APROBACION)
                    {
                        li.Add(i);
                    }
                }
            }
            return li;
        }
        public void CambiarEstadoMiembro(Administrador? unA, Miembro? unM)
        {
            if (unA != null && unA.GetType().Name == "Administrador")
            {
                if (unM != null)
                {
                    if (unM.Estado == EnumEstadoMiembro.NO_BLOQUEADO)
                    {
                        unM.Estado = EnumEstadoMiembro.BLOQUEADO;
                    }
                    else
                    {
                        unM.Estado = EnumEstadoMiembro.NO_BLOQUEADO;
                    }
                }
            }
        }
        public void CambiarEstadoPost(Administrador unA, int Id)
        {
            Post? post = DevolverPost(Id);
            if (unA != null && unA.GetType().Name == "Administrador")
            {
                if (post != null)
                {
                    if (post.Estado == EnumEstadoPost.DESHABILITADO)
                    {
                        post.Estado = EnumEstadoPost.HABILITADO;
                    }
                    else
                    {
                        post.Estado = EnumEstadoPost.DESHABILITADO;
                    }
                }
            }
        }
        public Invitacion CrearInvitacion(Miembro mSolicitante, Miembro mSolicitado)
        {
            Invitacion unaI = new Invitacion(EnumEstadoInvitacion.PENDIENTE_APROBACION, mSolicitante, mSolicitado, DateTime.Now);
            return unaI;
        }
        public void AceptarSolicitud(Invitacion unaI)
        {
            if (unaI != null)
            {
                unaI.EstadoInvitacion = EnumEstadoInvitacion.APROBADA;
                Miembro Solicitante = (Miembro)DevolverUsuario(unaI.Solicitante.Email);
                Miembro Solicitado = (Miembro)DevolverUsuario(unaI.Solicitado.Email);
                Solicitante.Amigos.Add(Solicitado);
                Solicitado.Amigos.Add(Solicitante);
            }
        }
        public void RechazarSolicitud(Invitacion unaI)
        {
            if (unaI != null)
            {
                unaI.EstadoInvitacion = EnumEstadoInvitacion.RECHAZADA;
            }
        }
        public void CrearPost(string nombreImagen,
        EnumVisibilidad visibilidad, string titulo,
        Miembro autor, string contenido)
        {
            if (autor != null)
            {
                // Datos por defecto que no se piden por parámetro
                List<Comentario> comentarios = new List<Comentario>() { };
                EnumEstadoPost estado = EnumEstadoPost.HABILITADO;
                DateTime fecha = DateTime.Now;
                List<Reaccion> reacciones = new List<Reaccion>();
                Post unP = new Post(nombreImagen, visibilidad, comentarios,
                    estado, titulo, fecha, autor, contenido, reacciones);
                AgregarPublicacion(unP);
            }
        }
        public bool PostEsPublico(Post post)
        {
            if (post.Visibilidad == EnumVisibilidad.PUBLICO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PostEsDeAmigo(Miembro unM, Post post)
        {
            Miembro autor = post.Autor;
            if (unM.EsAmigo(autor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PostEsDeMiembro(Miembro unM, Post post)
        {
            if (post.Autor.Email == unM.Email)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PostDeshabilitado(Post post)
        {
            if (post.Estado == EnumEstadoPost.DESHABILITADO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Post> ListarPostsVisiblesMiembro(Miembro unM)
        {
            List<Post> lp = new List<Post>();
            foreach (Post p in MostrarPosts())
            {
                if (!PostDeshabilitado(p))
                {
                    if (PostEsPublico(p) || PostEsDeMiembro(unM, p) || PostEsDeAmigo(unM, p))
                    {
                        lp.Add(p);
                    }
                }
            }
            return lp;
        }
        public Reaccion CrearLike(Miembro unM)
        {
            return new Reaccion(EnumTipoReaccion.LIKE, unM);
        }
        public Reaccion CrearDislike(Miembro unM)
        {
            return new Reaccion(EnumTipoReaccion.DISLIKE, unM);
        }
        public void MiembroDarLike(Miembro unM, Publicacion unaP)
        {
            if (unaP.MiembroPuedeReaccionar(unM))
            {
                Reaccion unLike = CrearLike(unM);
                unaP.Reacciones.Add(unLike);
            }
        }
        public void MiembroDarDislike(Miembro unM, Publicacion unaP)
        {
            if (unaP.MiembroPuedeReaccionar(unM))
            {
                Reaccion unDislike = CrearDislike(unM);
                unaP.Reacciones.Add(unDislike);
            }
        }
        public Publicacion? DevolverPublicacion(int id)
        {
            foreach (Publicacion unaP in Publicaciones())
            {
                if (unaP is Post unPost)
                {
                    foreach (Comentario unC in unPost.Comentarios)
                    {
                        if (unC.Id == id) return (Publicacion)unC;
                    }
                }
                if (unaP.Id == id) return unaP;
            }
            return null;
        }
        // El metodo ListarPublicacionesDesagregadasVisiblesMiembro
        // tiene por objetivo devolver una lista de publicaciones (post y comentarios)
        // que esten habilitadas para ser vistas por un miembro. 
        // En su logica, este metodo extrae del interior de cada post sus comentarios
        // (lista de Comentario en objeto Post) y los agrega a una lista auxiliar, donde
        // quedan a disposicion tanto los objetos Post como Comentario que el miembro
        // esta habilitado para ver (es decir, que cumplen las condiciones que se contemplan
        // en el metodo ListarPostsVisiblesMiembro)
        public List<Publicacion> ListarPublicacionesDesagregadasVisiblesMiembro(Miembro unM)
        {
            List<Post> publicaciones = ListarPostsVisiblesMiembro(unM);
            List<Publicacion> aux = new List<Publicacion>();
            foreach (Publicacion publicacion in publicaciones)
            {
                aux.Add(publicacion);
                if (publicacion is Post post)
                {
                    foreach (Comentario c in post.Comentarios)
                    {
                        aux.Add(c);
                    }
                }
            }
            return aux;
        }
        public bool BuscarTextoEnPublicacion(string texto, Publicacion unaP)
        {
            // se introduce un control para gestionar si se hace una lectura null desde input html
            if (texto == null) texto = "";
            if (unaP.Contenido.IndexOf(texto) != -1)
            {
                return true;
            }
            return false;
        }
        public bool PublicacionVaEsMayor(Publicacion unaP, int va)
        {
            // Se agregan las primeras dos condiciones para gestionar una lectura null de input html
            // y para devolver casos si se ingresa valor 0 (porque de lo contrario se imposibilita ver
            // publicaciones con VA 0 - se elimina gestión de null, parece que parsea a 0 directamente -.
            if (va == 0 || unaP.ValorAceptacion(unaP) > va)
            {
                return true;
            }
            return false;
        }
        public bool PublicacionContieneTextoFiltradoVA(string texto, int va, Publicacion unaP)
        {
            if (PublicacionVaEsMayor(unaP, va))
            {
                if (BuscarTextoEnPublicacion(texto, unaP))
                {
                    return true;
                }
            }
            return false;
        }
        public List<Publicacion> ListarPublicacionesVisiblesTextoFiltradoVA(string texto, int va, Miembro unM)
        {
            List<Publicacion> publicaciones = ListarPublicacionesDesagregadasVisiblesMiembro(unM);
            List<Publicacion> aux = new List<Publicacion>();
            foreach (Publicacion p in publicaciones)
            {
                Console.WriteLine(p);
                if (PublicacionContieneTextoFiltradoVA(texto, va, p))
                {
                    aux.Add(p);
                }
            }
            return aux;
        }
        public bool NombreImagenEsValido(string nombre)
        {
            bool esJPG = nombre.IndexOf(".jpg") != -1;
            bool esPNG = nombre.IndexOf(".png") != -1;
            if (esJPG || esPNG)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Metodo para precarga
        private void Precarga()
        {
            try
            {
                Administrador a1 = new Administrador("admin1@sitio.com", "admin.1");
                Administrador a2 = new Administrador("admin2@sitio.com", "admin.2");
                Miembro m1 = new Miembro("Ezequiel", "Pérez", new DateTime(2001, 2, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro1@sitio.com", "miembro.1");
                Miembro m2 = new Miembro("Ximena", "González", new DateTime(2001, 10, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro2@sitio.com", "miembro.2");
                Miembro m3 = new Miembro("Leandro", "Pérez", new DateTime(2001, 2, 5), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro3@sitio.com", "miembro.3");
                Miembro m4 = new Miembro("Yolanda", "García", new DateTime(2003, 6, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro4@sitio.com", "miembro.4");
                Miembro m5 = new Miembro("Matías", "López", new DateTime(2008, 2, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro5@sitio.com", "miembro.5");
                Miembro m6 = new Miembro("Zenaida", "Pérez", new DateTime(2003, 10, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro6@sitio.com", "miembro.6");
                Miembro m7 = new Miembro("Aarón", "González", new DateTime(2007, 2, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro7@sitio.com", "miembro.7");
                Miembro m8 = new Miembro("Giselle", "González", new DateTime(2002, 2, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro8@sitio.com", "miembro.8");
                Miembro m9 = new Miembro("Baltasar", "Pérez", new DateTime(2007, 10, 3), new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, "miembro9@sitio.com", "miembro.9");
                Miembro m10 = new Miembro("Jazmín", "Torres", new DateTime(2003, 7, 7), new List<Miembro>(), EnumEstadoMiembro.BLOQUEADO, "miembro10@sitio.com", "miembro.10");
                // Se asignan objetos tipo Miembro a las diferentes listas de amigos de los objetos Miembro
                m1.Amigos.Add(m2);
                m1.Amigos.Add(m3);
                m1.Amigos.Add(m4);
                m1.Amigos.Add(m5);
                m1.Amigos.Add(m6);
                m1.Amigos.Add(m7);
                m1.Amigos.Add(m8);
                m1.Amigos.Add(m9);
                m1.Amigos.Add(m10);
                m2.Amigos.Add(m1);
                m3.Amigos.Add(m1);
                m4.Amigos.Add(m1);
                m5.Amigos.Add(m1);
                m6.Amigos.Add(m1);
                m7.Amigos.Add(m1);
                m8.Amigos.Add(m1);
                m9.Amigos.Add(m1);
                m10.Amigos.Add(m1);
                m2.Amigos.Add(m3);
                m2.Amigos.Add(m4);
                m2.Amigos.Add(m5);
                m2.Amigos.Add(m6);
                m2.Amigos.Add(m7);
                m2.Amigos.Add(m8);
                m2.Amigos.Add(m9);
                m2.Amigos.Add(m10);
                m3.Amigos.Add(m2);
                m4.Amigos.Add(m2);
                m5.Amigos.Add(m2);
                m6.Amigos.Add(m2);
                m7.Amigos.Add(m2);
                m8.Amigos.Add(m2);
                m9.Amigos.Add(m2);
                m10.Amigos.Add(m2);
                m9.Amigos.Add(m10);
                m10.Amigos.Add(m9);
                m10.Amigos.Add(m6);
                m6.Amigos.Add(m10);
                AgregarUsuario(a1);
                AgregarUsuario(a2);
                AgregarUsuario(m1);
                AgregarUsuario(m2);
                AgregarUsuario(m3);
                AgregarUsuario(m4);
                AgregarUsuario(m5);
                AgregarUsuario(m6);
                AgregarUsuario(m7);
                AgregarUsuario(m8);
                AgregarUsuario(m9);
                AgregarUsuario(m10);
                Invitacion i = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m2, DateTime.Now);
                Invitacion i2 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m3, DateTime.Now);
                Invitacion i3 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m4, DateTime.Now);
                Invitacion i4 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m5, DateTime.Now);
                Invitacion i5 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m6, DateTime.Now);
                Invitacion i6 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m7, DateTime.Now);
                Invitacion i7 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m8, DateTime.Now);
                Invitacion i8 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m9, DateTime.Now);
                Invitacion i9 = new Invitacion(EnumEstadoInvitacion.APROBADA, m1, m10, DateTime.Now);
                Invitacion i10 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m1, DateTime.Now);
                Invitacion i11 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m3, DateTime.Now);
                Invitacion i12 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m4, DateTime.Now);
                Invitacion i13 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m5, DateTime.Now);
                Invitacion i14 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m6, DateTime.Now);
                Invitacion i15 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m7, DateTime.Now);
                Invitacion i16 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m8, DateTime.Now);
                Invitacion i17 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m9, DateTime.Now);
                Invitacion i18 = new Invitacion(EnumEstadoInvitacion.APROBADA, m2, m10, DateTime.Now);
                Invitacion i19 = new Invitacion(EnumEstadoInvitacion.PENDIENTE_APROBACION, m3, m4, DateTime.Now);
                Invitacion i20 = new Invitacion(EnumEstadoInvitacion.RECHAZADA, m4, m5, DateTime.Now);
                Invitacion i21 = new Invitacion(EnumEstadoInvitacion.PENDIENTE_APROBACION, m5, m6, DateTime.Now);
                Invitacion i22 = new Invitacion(EnumEstadoInvitacion.PENDIENTE_APROBACION, m6, m7, DateTime.Now);
                Invitacion i23 = new Invitacion(EnumEstadoInvitacion.RECHAZADA, m7, m8, DateTime.Now);
                Invitacion i24 = new Invitacion(EnumEstadoInvitacion.RECHAZADA, m8, m9, DateTime.Now);
                Invitacion i25 = new Invitacion(EnumEstadoInvitacion.APROBADA, m9, m10, DateTime.Now);
                Invitacion i26 = new Invitacion(EnumEstadoInvitacion.APROBADA, m10, m6, DateTime.Now);
                AgregarInvitacion(i);
                AgregarInvitacion(i2);
                AgregarInvitacion(i3);
                AgregarInvitacion(i4);
                AgregarInvitacion(i5);
                AgregarInvitacion(i6);
                AgregarInvitacion(i7);
                AgregarInvitacion(i8);
                AgregarInvitacion(i9);
                AgregarInvitacion(i10);
                AgregarInvitacion(i11);
                AgregarInvitacion(i12);
                AgregarInvitacion(i13);
                AgregarInvitacion(i14);
                AgregarInvitacion(i15);
                AgregarInvitacion(i16);
                AgregarInvitacion(i17);
                AgregarInvitacion(i18);
                AgregarInvitacion(i19);
                AgregarInvitacion(i20);
                AgregarInvitacion(i21);
                AgregarInvitacion(i22);
                AgregarInvitacion(i23);
                AgregarInvitacion(i24);
                AgregarInvitacion(i25);
                AgregarInvitacion(i26);
                Post p1 = new Post("Nombre imagen post 1", EnumVisibilidad.PUBLICO, new List<Comentario>(), EnumEstadoPost.HABILITADO, "Titulo de post 1", new DateTime(2023, 10, 01), m2, "Contenido de post 1", new List<Reaccion>());
                Post p2 = new Post("Nombre imagen post 2", EnumVisibilidad.PUBLICO, new List<Comentario>(), EnumEstadoPost.HABILITADO, "Titulo de post 2", new DateTime(2023, 3, 01), m3, "Contenido de post 2", new List<Reaccion>());
                Post p3 = new Post("Nombre imagen post 3", EnumVisibilidad.PUBLICO, new List<Comentario>(), EnumEstadoPost.HABILITADO, "Titulo de post 3", new DateTime(2023, 2, 04), m2, "Contenido de post 3", new List<Reaccion>());
                Post p4 = new Post("Nombre imagen post 4", EnumVisibilidad.PUBLICO, new List<Comentario>(), EnumEstadoPost.HABILITADO, "Titulo de post 4", new DateTime(2023, 1, 01), m4, "Contenido de post 4", new List<Reaccion>());
                Post p5 = new Post("Nombre imagen post 5", EnumVisibilidad.PUBLICO, new List<Comentario>(), EnumEstadoPost.HABILITADO, "Titulo de post 5", new DateTime(2023, 5, 01), m4, "Contenido de post 5", new List<Reaccion>());
                Comentario c1 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c1", new DateTime(2023, 10, 01), m2, "Contenido c1", new List<Reaccion>());
                Comentario c2 = new Comentario(EnumVisibilidad.PRIVADO, "Titulo c2", new DateTime(2023, 10, 01), m3, "Contenido c2", new List<Reaccion>());
                Comentario c3 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c3", new DateTime(2023, 10, 01), m5, "Contenido c3", new List<Reaccion>());
                Comentario c4 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c4", new DateTime(2023, 10, 01), m4, "Contenido c4", new List<Reaccion>());
                Comentario c5 = new Comentario(EnumVisibilidad.PRIVADO, "Titulo c5", new DateTime(2023, 10, 01), m8, "Contenido c5", new List<Reaccion>());
                Comentario c6 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c6", new DateTime(2023, 10, 01), m8, "Contenido c6", new List<Reaccion>());
                Comentario c7 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c7", new DateTime(2023, 10, 01), m8, "Contenido c7", new List<Reaccion>());
                Comentario c8 = new Comentario(EnumVisibilidad.PRIVADO, "Titulo c8", new DateTime(2023, 10, 01), m6, "Contenido c8", new List<Reaccion>());
                Comentario c9 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c9", new DateTime(2023, 10, 01), m8, "Contenido c9", new List<Reaccion>());
                Comentario c10 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c10", new DateTime(2023, 10, 01), m4, "Contenido c10", new List<Reaccion>());
                Comentario c11 = new Comentario(EnumVisibilidad.PRIVADO, "Titulo c11", new DateTime(2023, 10, 01), m2, "Contenido c11", new List<Reaccion>());
                Comentario c12 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c12", new DateTime(2023, 10, 01), m4, "Contenido c12", new List<Reaccion>());
                Comentario c13 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c13", new DateTime(2023, 10, 01), m5, "Contenido c13", new List<Reaccion>());
                Comentario c14 = new Comentario(EnumVisibilidad.PRIVADO, "Titulo c14", new DateTime(2023, 10, 01), m9, "Contenido c14", new List<Reaccion>());
                Comentario c15 = new Comentario(EnumVisibilidad.PUBLICO, "Titulo c15", new DateTime(2023, 10, 01), m5, "Contenido c15", new List<Reaccion>());
                Reaccion r1 = new Reaccion(EnumTipoReaccion.LIKE, m1);
                Reaccion r2 = new Reaccion(EnumTipoReaccion.DISLIKE, m2);
                Reaccion r3 = new Reaccion(EnumTipoReaccion.LIKE, m3);
                Reaccion r4 = new Reaccion(EnumTipoReaccion.LIKE, m4);
                p1.Reacciones.Add(r3);
                p3.Reacciones.Add(r4);
                c3.Reacciones.Add(r1);
                c3.Reacciones.Add(r2);
                AgregarComentario(c1, p1);
                AgregarComentario(c2, p1);
                AgregarComentario(c3, p1);
                AgregarComentario(c4, p2);
                AgregarComentario(c5, p2);
                AgregarComentario(c6, p2);
                AgregarComentario(c7, p3);
                AgregarComentario(c8, p3);
                AgregarComentario(c9, p3);
                AgregarComentario(c10, p4);
                AgregarComentario(c11, p4);
                AgregarComentario(c12, p4);
                AgregarComentario(c13, p5);
                AgregarComentario(c14, p5);
                AgregarComentario(c15, p5);
                AgregarPublicacion(p1);
                AgregarPublicacion(p2);
                AgregarPublicacion(p3);
                AgregarPublicacion(p4);
                AgregarPublicacion(p5);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
