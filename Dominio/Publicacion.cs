using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public abstract class Publicacion : IComparable<Publicacion>
    {
        #region Atributos
        private static int _ultimoId = 0;
        private int _id;
        private string _titulo;
        private DateTime _fecha;
        private Miembro _autor;
        private string _contenido;
        private List<Reaccion> _reacciones;
        #endregion
        #region Propiedades
        public static int UltimoId { get => _ultimoId; set => _ultimoId = value; }
        public int Id { get => _id; }
        public string Titulo { get => _titulo; set => _titulo = value; }
        public DateTime Fecha { get => _fecha; set => _fecha = value; }
        public Miembro Autor { get => _autor; set => _autor = value; }
        public string Contenido { get => _contenido; set => _contenido = value; }
        public List<Reaccion> Reacciones { get => _reacciones; set => _reacciones = value; }
        #endregion
        #region Métodos
        public int CompareTo(Publicacion? otraPublicacion)
        {
            return -this._titulo.CompareTo(otraPublicacion._titulo);
        }
        public override string ToString()
        {
            return $"Tipo publicación: {GetType().Name} - Título: {_titulo} - Email autor: {_autor.Email}";
        }
        public virtual void Validar()
        {
            ValidarContenido();
            ValidarTitulo();
        }
        private void ValidarContenido()
        {
            if (Contenido.Length == 0)
            {
                throw new Exception("Error. La publicación no puede estar vacía.");
            }
        }
        private void ValidarTitulo()
        {
            if (Titulo.Length == 0)
            {
                throw new Exception("Error. El título no puede estar vacío.");
            }
            if (Titulo.Length < 3)
            {
                throw new Exception("Error. El título debe tener al menos tres caracteres.");
            }
        }
        public int CantidadLikes(Publicacion publicacion)
        {
            int aux = 0;
            foreach (Reaccion r in publicacion.Reacciones)
            {
                if (r.TipoReaccion == EnumTipoReaccion.LIKE)
                {
                    aux++;
                }
            }
            return aux;
        }
        public int CantidadDislikes(Publicacion publicacion)
        {
            int aux = 0;
            foreach (Reaccion r in publicacion.Reacciones)
            {
                if (r.TipoReaccion == EnumTipoReaccion.DISLIKE)
                {
                    aux++;
                }
            }
            return aux;
        }
        public virtual int ValorAceptacion(Publicacion publicacion)
        {
            int VA = (CantidadLikes(publicacion) * 5) + (CantidadDislikes(publicacion) * -2);
            return VA;
        }
        public bool PublicacionTieneReaccionDeMiembro(Miembro unM)
        {
            foreach (Reaccion r in Reacciones)
            {
                bool Prueba = r.ReaccionEsDeMiembro(unM);
                if (r.ReaccionEsDeMiembro(unM))
                {
                    return true;
                }
            }
            return false;
        }
        public bool MiembroPuedeReaccionar(Miembro unM)
        {
            return !PublicacionTieneReaccionDeMiembro(unM);
        }
        #endregion
        #region Constructores
        public Publicacion(string titulo, DateTime fecha, Miembro autor, string contenido, List<Reaccion> reacciones)
        {
            _id = UltimoId++;
            Titulo = titulo;
            Fecha = fecha;
            Autor = autor;
            Contenido = contenido;
            Reacciones = reacciones;
        }
        public Publicacion()
        {
        }
        #endregion
    }
}
