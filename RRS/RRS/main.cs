using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRS
{
    class main
    {
        public static Graph g = Graph.GetInstance();

        public static Queue<Vertex> q = new Queue<Vertex>();

        static void Main(string[] args)
        {
            #region FILE로 Graph구성
            FileStream fs = new FileStream("input.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string edge;

            string[] vertices = sr.ReadLine().Split(' ');

            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex v = new Vertex();
                g.addVertex(v, i.ToString());
            }

            while((edge = sr.ReadLine()) != null){
                string from, to;
                int capa;

                from = edge.Split(' ')[0].ToString();
                to = edge.Split(' ')[1].ToString();
                capa = Convert.ToInt32(edge.Split(' ')[2]);

                g.addEdges(from, to, capa);
            }

            g.printGraph();
            #endregion FILE로 Graph구성
            // DB 호출 -> _dicGraphInfo -> List<Edge>

            #region BFS로 Routing
            Vertex start = g.getVertex("0");

            q.Enqueue(start);


            #endregion
        }
    }
}
