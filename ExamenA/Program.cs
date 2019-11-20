using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

namespace ExamenA
{
    class Program
    {
        static bool[] mesas;
        static void Main(string[] args)
        {
            // Inicializa una cantidad de mesas
            InicializarMesas();

            // Pipe
            NamedPipeServerStream npss = new NamedPipeServerStream("ExamenA", PipeDirection.InOut);

            // Streams
            StreamWriter sw = new StreamWriter(npss);
            StreamReader sr = new StreamReader(npss);

            // Inicia el proceso
            Process.Start(@"..\..\..\ExamenAHijo\bin\Debug\ExamenAHijo.exe");

            // Espera la conexión
            npss.WaitForConnection();
            sw.AutoFlush = true;

            // Lee la respuesta
            string resp = "";
            resp = sr.ReadLine();

            // Procesa la respuesta
            while (resp.ToUpper().CompareTo("FIN") != 0)
            {
                // Valida la respuesta y envia las mesas al cliente
                switch (resp.ToUpper())
                {
                    case "LIBERAR":
                        for (int i = 0; i < mesas.Length; i++)
                        {
                            if (!mesas[i])
                                sw.WriteLine(i.ToString());
                        }
                        break;
                    case "RESERVAR":
                        for (int i = 0; i < mesas.Length; i++)
                        {
                            if (mesas[i])
                                sw.WriteLine(i.ToString());
                        }
                        break;
                }

                // Envia la respuesta de fin
                sw.WriteLine("-1");

                // Recibe el número de mesa
                int index = Int32.Parse(sr.ReadLine());

                // Realiza una opción dependiendo de la respuesta y el indice
                sw.WriteLine(Accion(resp, index));

                // Recupera la siguiente opción
                resp = sr.ReadLine();
            }

            // Cierre de Pipe
            npss.Close();

            Console.ReadLine();
        }

        private static void InicializarMesas()
        {
            int numMesas;
            do
            {
                Console.WriteLine("Introduce un número de mesas: ");
                numMesas = Int32.Parse(Console.ReadLine());
                mesas = new bool[numMesas];
            } while (numMesas < 0);

            for (int i = 0; i < mesas.Length; i++)
                mesas[i] = true;
        }

 

        private static string Accion(string opcion, int index)
        {
            string resp = "";
            if (index >= 0 && index < mesas.Length)
            {
                switch (opcion)
                {
                    case "Liberar":
                        if (!mesas[index])
                        {
                            mesas[index] = false;
                            resp = $"Mesa {index} liberada.";
                        }
                        else
                        {
                            resp = $"Esta mesa no existe o no esta ocupada.";
                        }
                        break;
                    case "Reservar":
                        if (mesas[index])
                        {
                            mesas[index] = true;
                            resp = $"Mesa {index} reservada.";
                        }
                        else
                        {
                            resp = $"Esta mesa no existe o está reservada.";
                        }
                        break;

                }
            }
            else
            {
                resp = "mesa incorrecta";
            }

            return resp;
        }
    }
}
