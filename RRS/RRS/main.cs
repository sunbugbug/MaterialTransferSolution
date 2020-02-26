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

        public static Queue<Tuple<string, Route>> queue = new Queue<Tuple<string, Route>>();

        public static bool isFound;
        public static int MAXOFMIN;
        public static Route lastRoute;

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
            { /* BFS하기 전에 초기화 */
                queue.Clear();
                isFound = false;
                MAXOFMIN = (int)DEFAULT_NUM.MINNUM;
                lastRoute = null;
            }

            //Vertex v = g.getVertex("0");

            queue.Enqueue(new Tuple<String, Route>("0", new Route("0")));

            BFS("7");

            Console.WriteLine("MaxOfMin = {0}", MAXOFMIN);
            lastRoute.printRoute();

            #endregion
            return ;
        }

        public static void BFS(string End)
        {
            if (queue.Count() == 0) return;
            Tuple<String, Route> tuple = queue.Dequeue();

            Vertex vertex = g.getVertex(tuple.Item1);

            Route route = tuple.Item2;

            if (vertex.Equals(g.getVertex(End))){
                isFound = true;
            }

            if (!isFound)
            { /* 현재 vertex에서 이동 가능한 vertex들을 모드 Queue에 추가함 */
                foreach(Edge e in vertex.getAdjList())
                {
                    //queue.Enqueue(g.getVertex(e.getTo()));
                    Route newRoute = new Route(route);
                    newRoute.AddRoute(e);

                    Tuple<String, Route> nextTuple = new Tuple<String, Route>(e.getTo(), newRoute);
                    queue.Enqueue(nextTuple);
                }
            }
            else if (isFound)
            {
                if (MAXOFMIN < route.getMinEdge())
                {
                    MAXOFMIN = route.getMinEdge();
                    lastRoute = route;
                }
            }
            
            route.printRoute();

            BFS(End);

        }
    }

    enum DEFAULT_NUM
    {
        MAXNUM = 9999,
        MINNUM = -1
    }
}
