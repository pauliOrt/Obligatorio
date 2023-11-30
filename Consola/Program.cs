namespace Consola
{

    using Dominio;
    using System.Globalization;
    using System.Security.Cryptography;

    internal class Program
    {
        static void Main(string[] args)
        {
            OpcionesMenu();
        }


        #region Metodo para punto 1
       static void AgregarUsuario()
        {
            Sistema sistema = Sistema.Instancia;
            try
            {
                Console.WriteLine("Ingresa tu nombre: ");
                string nombre = Console.ReadLine();
                Console.WriteLine("Ingresa tu apellido: ");
                string apellido = Console.ReadLine();
                Console.WriteLine("Ingresa tu fecha de nacimiento (formato 'AAAA, MM, DD'): ");
                //DateTime fechaNacimiento = DateTime.Parse(Console.ReadLine());
                string fechaNacimiento = Console.ReadLine();
                if(DateTime.TryParse(fechaNacimiento, out DateTime fecha))
                {
                    // Parseo exitoso
                }
                else
                {
                    Console.WriteLine("Hubo un error con la lectura de la fecha introducida...");
                    Console.WriteLine("Recuerda que debes ingresa tu fecha de nacimiento en formato 'AAAA, MM, DD' ");
                    return;
                }

                Console.WriteLine("Ingresa tu email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Ingresa tu contrasenia: ");
                string contrasenia = Console.ReadLine();

                Miembro nuevoM = new Miembro(nombre, apellido, fecha, new List<Miembro>(), EnumEstadoMiembro.NO_BLOQUEADO, email, contrasenia);

                sistema.AgregarUsuario(nuevoM);
                Console.WriteLine($"La operación fue exitosa. Se agregó al sistema el miembro con email {email}");

                /* 
                foreach (Usuario u in sistema.Usuarios()) {

                    Console.WriteLine(u.Email);
                }
                */


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
        #endregion

        #region Metodo para punto 2
        static void ListarPublicacionesEnConsola()
        {

            Sistema sistema = Sistema.Instancia;

            Console.WriteLine("Ingresa un email de miembro para ver todas las publicaciones que haya realizado...");
            string email = Console.ReadLine();

            List<Publicacion> publicaciones = sistema.ListarPublicaciones(email);

            foreach (Publicacion p in publicaciones) {
                Console.WriteLine(p.ToString());
            }
            if(publicaciones.Count==0)
            {
                Console.WriteLine("No se encontraron publicaciones realizadas por el miembro con el email ingresado");
            }

        }

        #endregion

        #region Metodo para punto 3

        static void ListarPostConComentarioEmail()

        {
            Sistema sistema = Sistema.Instancia;

            Console.WriteLine("Ingresa un email de miembro para ver todos los post donde haya realizado comentarios...");
            string email = Console.ReadLine();

            List<Post> auxiliar = sistema.ListarPostComentariosEmail(email);

            foreach (Post p in auxiliar)
            {
                Console.WriteLine(p.ToString());

            }
            if (auxiliar.Count == 0)
            {
                Console.WriteLine("No se encontraron post con comentarios realizados por el miembro con el email ingresado");
            }
        }


        #endregion

        #region Metodo para punto 4
        static void ListarPostEntreFechas()
        {

            Sistema sistema = Sistema.Instancia;

            Console.WriteLine("Para ver todos los post realizados en un periodo determinado (descendentemente por titulo), ingresa dos fechas...");

            Console.WriteLine("Ingresa una fecha (en formato 'AAAA, MM, DD'): ");
            //DateTime fecha1 = DateTime.Parse(Console.ReadLine());

            string fechaTexto1 = Console.ReadLine();
            if (DateTime.TryParse(fechaTexto1, out DateTime fecha1))
            {
                // Parseo exitoso
            }
            else
            {
                Console.WriteLine("Hubo un error con la lectura de la fecha introducida...");
                Console.WriteLine("Recuerda que debes ingresa la fecha en formato 'AAAA, MM, DD' ");
                return;
            }

            Console.WriteLine("Ingresa otra fecha (en formato 'AAAA, MM, DD'): ");
            //DateTime fecha2 = DateTime.Parse(Console.ReadLine());

            string fechaTexto2 = Console.ReadLine();
            if (DateTime.TryParse(fechaTexto2, out DateTime fecha2))
            {
                // Parseo exitoso
            }
            else
            {
                Console.WriteLine("Hubo un error con la lectura de la fecha introducida...");
                Console.WriteLine("Recuerda que debes ingresa la fecha en formato 'AAAA, MM, DD' ");
                return;
            }

            DateTime aux = new DateTime();
            if(fecha1 > fecha2)
            {
                aux = fecha2;
                fecha2 = fecha1;
                fecha1 = aux;
            }

            List<Post> auxiliar = sistema.ListarPostPorFecha(fecha1, fecha2);

            foreach (Post p in auxiliar)
            {

                Console.WriteLine(p.MostrarDatosPost());
            }

            if(auxiliar.Count == 0)
            {
                Console.WriteLine("No se encontraron post para el perido indicado");

            }

        }
        #endregion

        #region Metodo para punto 5

        static void ListarTopPublicaciones()
        {
            Sistema sistema = Sistema.Instancia;

            List<Usuario> auxiliar = sistema.ListarTopMiembros();
            Console.WriteLine("Miembro/s con mas publicaciones: ");
            foreach(Usuario p in auxiliar)
            {
                Console.WriteLine(p.ToString());
            }
        }

        #endregion


        #region metodos Menu
        static int LeerEntero()
        {
            string texto = Console.ReadLine();
            int resultado = 0;
            while (!int.TryParse(texto, out resultado) || resultado < 0 || resultado > 5)
            {
                Console.Clear();
                Menu();
                Console.WriteLine("Error vuelva a ingresar el valor");

                texto = Console.ReadLine();
            }

            return resultado;
        }
        static void Menu()
        {
            string[] titulos = { "Salir del programa", "Dar de alta un miembro", "Listar todas las publicaciones por email",
                "Listar  por email los post que haya comentado", "Lista post que se haya reaizado entre dos fechas",
                "Obtener los miembros que hayan realizado mas publicaciones"};
            int option = 0;

            foreach (string titulo in titulos)
            {
                Console.WriteLine($"{option} - {titulo}");
                option++;
            }
        }

        static void OpcionesMenu()
        {
            int valor = -1;
            while (valor != 0)
            {
                Console.Clear();
                Menu();
                Console.WriteLine("Ingrese opcion del menu:");
                valor = LeerEntero();
                switch (valor)
                {
                    case 1:
                        //Punto1();
                        AgregarUsuario();
                        Console.ReadKey();
                        break;
                    case 2:
                        //Punto2();
                        ListarPublicacionesEnConsola();
                        Console.ReadKey();

                        break;
                    case 3:
                        //Punto3();
                        ListarPostConComentarioEmail();
                        Console.ReadKey();
                        break;
                    case 4:
                        //Punto4();
                        ListarPostEntreFechas();
                        Console.ReadKey();
                        break;
                    case 5:
                        //Punto5();
                        ListarTopPublicaciones();
                        Console.ReadKey();
                        break;
                    default:
                        break;

                }

            }
        }
        #endregion
    }

}