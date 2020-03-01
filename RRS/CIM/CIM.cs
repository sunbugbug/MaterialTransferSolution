using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ezRouting;

namespace CIM
{
    class CIM
    {
        public static Graph g = Graph.GetInstance();
        public static string MachineNo;
        public static string MachineName;
        public static void Main(string[] args)
        {
            MachineNo = args[0];
            MachineName = args[1];

            #region FILE로 Graph구성 -> DB로 대체예정
            FileStream fs = new FileStream("input.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            makeGraph(sr);

            #endregion



            // RRS와 통신연결을 하고 필요한 정보들을 보냄
        }

        public static void makeGraph(StreamReader sr)
        {
            string edge;

            string[] vertices = sr.ReadLine().Split(' ');

            Console.WriteLine("Graph 구성중..");
            Console.WriteLine("");

            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex v = new Vertex();
                g.addVertex(v, i.ToString());
            }

            while((edge = sr.ReadLine()) != null)
            {
                string from, to;
                int capa;

                from = edge.Split(' ')[0].ToString();
                to = edge.Split(' ')[1].ToString();
                capa = Convert.ToInt32(edge.Split(' ')[2]);

                g.addEdges(from, to, capa);

                g.printGraph();

                Console.WriteLine("");
                Console.WriteLine("Graph 구성 완료!");
                Console.WriteLine("--------------------------");
            }
        }
    }
}
