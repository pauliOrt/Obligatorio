using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class Comentario : Publicacion
    {
        #region Atributos
        private EnumVisibilidad _visibilidad;
        #endregion
        #region Propiedades
        public EnumVisibilidad Visibilidad { get => _visibilidad; set => _visibilidad = value; }
        #endregion
        #region Métodos
        #endregion
        #region Constructores
        public Comentario(EnumVisibilidad visibilidad,
            string titulo, DateTime fecha, 
            Miembro autor, string contenido, 
            List<Reaccion> reacciones): base(titulo, fecha,
                autor, contenido, reacciones)
        {
            Visibilidad = visibilidad;
        }
        public Comentario()
        {
        }
        #endregion
    }
}
