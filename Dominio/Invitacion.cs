using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio
{
    public class Invitacion
    {
        #region Atributos
        private EnumEstadoInvitacion _estadoInvitacion;
        private static int _ultimoId = 0;
        private int _id;
        private Miembro _solicitante;
        private Miembro _solicitado;
        private DateTime _fechaSolicitud;
        #endregion
        #region Propiedades
        public EnumEstadoInvitacion EstadoInvitacion { get => _estadoInvitacion; set => _estadoInvitacion = value; }
        public static int UltimoId { get => _ultimoId; set => _ultimoId = value; }
        public int Id { get => _id; set => _id = value; }
        public Miembro Solicitante { get => _solicitante; set => _solicitante = value; }
        public Miembro Solicitado { get => _solicitado; set => _solicitado = value; }
        public DateTime FechaSolicitud { get => _fechaSolicitud; set => _fechaSolicitud = value; }
        #endregion
        #region Métodos
        #endregion
        #region Constructores
        public Invitacion(EnumEstadoInvitacion estadoInvitacion, Miembro solicitante, Miembro solicitado, DateTime fechaSolicitud)
        {
            EstadoInvitacion = estadoInvitacion;
            Solicitante = solicitante;
            Solicitado = solicitado;
            FechaSolicitud = fechaSolicitud;
            Id = UltimoId++;
        }
        public Invitacion()
        {
        }
        #endregion
    }
}
