using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public abstract class Usuario: IValidable
    {
        #region Atributos
        private string _email;
        private string _contrasenia;
        #endregion
        #region Propiedades
        public string Email { get => _email; set => _email = value; }
        public string Contrasenia { get => _contrasenia; set => _contrasenia = value; }
        #endregion
        #region Métodos
        public virtual void Validar()
        {
            ValidarEmail();
            ValidarContrasenia();
        }
        private void ValidarEmail()
        {
            if(string.IsNullOrEmpty(_email) )
            {
                throw new Exception("Error. El email no puede ser vacío.");
            }
        }
        private void ValidarContrasenia()
        {
            if (string.IsNullOrEmpty(_contrasenia))
            {
                throw new Exception("Error. La contrasenia no puede ser vacía.");
            }
        }
        public override bool Equals(object? obj)
        {
            Usuario usuario = obj as Usuario;
            return this.Email == usuario.Email;
        }
        public abstract string ObtenerRol();
        #endregion
        #region Constructores
        public Usuario(string email, string contrasenia)
        {
            Email = email;
            Contrasenia = contrasenia;
        }
          public Usuario()
        {
        }
    }
    #endregion
}
