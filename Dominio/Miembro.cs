using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class Miembro : Usuario, IComparable<Miembro>
    {
        #region Atributos
        private string _nombre;
        private string _apellido;
        private DateTime _fechaDeNacimiento;
        private List<Miembro> _amigos;
        private EnumEstadoMiembro _estado;
        #endregion
        #region Propiedades
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Apellido { get => _apellido; set => _apellido = value; }
        public DateTime FechaDeNacimiento { get => _fechaDeNacimiento; set => _fechaDeNacimiento = value; }
        public EnumEstadoMiembro Estado { get => _estado; set => _estado = value; }
        public List<Miembro> Amigos { get => _amigos; set => _amigos = value; }
        #endregion
        #region Métodos
        public override string ToString()
        {
            return $"Email: {base.Email}- Nombre: {Nombre} - Apellido: {Apellido} - Fecha de nacimiento: {FechaDeNacimiento} - Estado: {Estado}";
        }
        public override void Validar()
        {
            base.Validar();
            ValidarNombre();
            ValidarApellido();
        }
        private void ValidarNombre()
        {
            if (string.IsNullOrEmpty(_nombre) || _nombre.Length < 3)
            {
                throw new Exception("Error. El nombre no puede ser vacío o menor a 3 caracteres.");
            }
        }
        private void ValidarApellido()
        {
            if (string.IsNullOrEmpty(_apellido))
            {
                throw new Exception("Error. El apellido no puede ser vacío.");
            }
        }
        public int CompareTo(Miembro unM)
        {
            int comparar = Apellido.CompareTo(unM.Apellido);
            if (comparar == 0)
            {
                return Nombre.CompareTo(unM.Nombre);
            }
            return comparar;
        }
        public override bool Equals(object? obj)
        {
            Miembro unM = obj as Miembro;
            return Email == unM.Email;
        }
        public bool EsAmigo(Miembro unM)
        {
           return Amigos.Contains(unM);
        }
        #endregion
        #region Constructores
        public Miembro(string nombre, string apellido, 
            DateTime fechaDeNacimiento, List<Miembro> amigos, 
            EnumEstadoMiembro estado, string email, 
            string contrasenia) : base (email, contrasenia)
        {
            Nombre = nombre;
            Apellido = apellido;
            FechaDeNacimiento = fechaDeNacimiento;
            Amigos = amigos;
            Estado = estado;
        }
        public Miembro()
        {
        }
        public override string ObtenerRol()
        {
            return "Miembro";
        }
        #endregion
    }
}
