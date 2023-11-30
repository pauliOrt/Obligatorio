using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class Post : Publicacion
    {
        #region Atributos
        private string _nombreImagen;
        private EnumVisibilidad _visibilidad;
        private List<Comentario> _comentarios;
        private EnumEstadoPost _estado;
        #endregion
        #region Propiedades
        public string NombreImagen { get => _nombreImagen; set => _nombreImagen = value; }
        public EnumVisibilidad Visibilidad { get => _visibilidad; set => _visibilidad = value; }
        public List<Comentario> Comentarios { get => _comentarios; set => _comentarios = value; }
        public EnumEstadoPost Estado { get => _estado; set => _estado = value; }
        #endregion
        #region Métodos
        public override void Validar()
        {
            base.Validar();
            ValidarNombreImagen();
        }
        // Funcion en desarrollo
        // No logramos implementar este control. Llevamos logica a webapp, a la vista de crear post el control de
        // verificar la extension... En Sistema implementamos el metodo.
        // bool esJPG = NombreImagen.IndexOf(".jpg") != -1;
        // bool esPNG = NombreImagen.IndexOf(".png") != -1;
        //  if (!esJPG || !esPNG)
        //   {
        //       "Error. El nombre de imagen debe terminar en '.jpg' o '.png.";
        //   }
        private void ValidarNombreImagen()
        {
            if (NombreImagen.Length == 0)
            {
                throw new Exception("Error. El nombre de la imagen no puede estar vacío.");
            }
            int posicionInicio = (NombreImagen.Length - 4);
            int posicionFinal = NombreImagen.Length;
            string ultimos4caracteres = NombreImagen.Substring(posicionInicio, posicionFinal);
            if (ultimos4caracteres != ".jpg" && ultimos4caracteres != ".png")
            {
                throw new Exception("Error. El nombre de imagen debe terminar en '.jpg' o '.png.");
            }
        }
        public string MostrarDatosPost()
        {
            return $"Id: {base.Id} - Fecha: {base.Fecha} - Título: {base.Titulo} - Contenido: {base.Contenido}";
        }
        public List<Comentario> ListarComentariosPost(Post post)
        {
            return post.Comentarios;
        }
        public override int ValorAceptacion(Publicacion publicacion)
        {
            int VA = base.ValorAceptacion(publicacion);
            Post post = (Post)publicacion;
            if (post.Visibilidad == EnumVisibilidad.PUBLICO)
            {
                VA += 10;
            }
            return VA;
        }
        #endregion
        #region Constructores
        public Post(string nombreImagen,
        EnumVisibilidad visibilidad,
        List<Comentario> comentarios,
        EnumEstadoPost estado, string titulo, DateTime fecha,
         Miembro autor, string contenido,
         List<Reaccion> reacciones) : base(titulo, fecha,
             autor, contenido, reacciones)
        {
            NombreImagen = nombreImagen;
            Visibilidad = visibilidad;
            Comentarios = comentarios;
            Estado = estado;
        }
        public Post()
        {
        }
        #endregion
    }
}
