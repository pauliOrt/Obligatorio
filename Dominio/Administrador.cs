using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Dominio
{
    public class Administrador : Usuario
    {
        #region Atributos
        #endregion
        #region Propiedades
        #endregion
        #region Métodos
        public override string ObtenerRol()
        {
            return "Administrador";
        }
        #endregion
        #region Constructores
        public Administrador (string email,
        string contrasenia) : base(email, contrasenia)
        {
        }
        public Administrador()
        {
        }
        #endregion
    }
}
