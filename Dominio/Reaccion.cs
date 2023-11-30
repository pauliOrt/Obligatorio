using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Reaccion    {
        #region Atributos
        private EnumTipoReaccion _tipoReaccion;
        private Miembro _miembro;
        #endregion
        #region Propiedades
        public EnumTipoReaccion TipoReaccion { get => _tipoReaccion; set => _tipoReaccion = value; }
        public Miembro Miembro { get => _miembro; set => _miembro = value; }
        #endregion
        #region Métodos
        public bool ReaccionEsDeMiembro(Miembro unM)
        {
            return unM.Email == Miembro.Email;
        }
        #endregion
        #region Constructores
        public Reaccion(EnumTipoReaccion tipoReaccion, Miembro miembro)
        {
            TipoReaccion = tipoReaccion;
            Miembro = miembro;
        }
        public Reaccion()
        {
        }
        #endregion
    }
}
