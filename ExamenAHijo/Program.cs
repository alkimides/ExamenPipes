using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;


namespace ExamenAHijo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pipe cliente
            NamedPipeClientStream npcs = new NamedPipeClientStream(".", "ExamenA", PipeDirection.InOut);

            // Conexión al servidor
            npcs.Connect();

            // Streams
            StreamReader sr = new StreamReader(npcs);
            StreamWriter sw = new StreamWriter(npcs);
            sw.AutoFlush = true;

            string opc = "";
            string resp = "";
            int cont = 0;

            // Pide una opción y se la envía al servidor
            opc = PedirOpcion();
            sw.WriteLine(opc);


            // Mientras no sea fin, se envia una respuesta al servidor
            while (opc.ToUpper().CompareTo("FIN") != 0)
            {
                cont = 0;

                // Mostrar mesas
                do
                {
                    resp = sr.ReadLine();
                    if (resp.CompareTo("-1") != 0)
                    {
                        cont++;
                        Console.WriteLine(resp);

                    }
                } while (resp.CompareTo("-1") != 0);

                // Comprueba si existen mesas para esa opción
                if (cont > 0)
                {
                    // Enviar mesa
                    Console.WriteLine("Selecciona una mesa: ");
                    sw.WriteLine(Console.ReadLine());

                    // Muestra la respuesta a la acción
                    Console.WriteLine(sr.ReadLine());
                }
                else
                {
                    Console.WriteLine("No existe mesas para esa opción");
                }

                // Pide una opción y se la envía al servidor

                opc = PedirOpcion();
                sw.WriteLine(opc);

            }

            // Cerrar pipe
            npcs.WaitForPipeDrain();
            npcs.Close();
        }

        //opciones del menu
        private static void MostrarMenu()
        {
            Console.WriteLine("1.Liberar");
            Console.WriteLine("2.Reservar");
            Console.WriteLine("3.Fin");
        }

        private static string PedirOpcion()
        {
            string opc = "";
            do
            {
                MostrarMenu();
                Console.WriteLine("Introduce una opción: ");
                opc = Console.ReadLine();
                Console.Clear();
            } while (opc.ToUpper().CompareTo("LIBERAR") != 0 && opc.ToUpper().CompareTo("RESERVAR") != 0 && opc.ToUpper().CompareTo("FIN") != 0);
            return opc;
        }

    }
}
