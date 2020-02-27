using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ezRouting;

namespace RRS
{
    public class RRS
    {
        public static Graph g = Graph.GetInstance();

        public static Queue<Tuple<string, Route>> queue = new Queue<Tuple<string, Route>>();

        public static bool isFound;
        public static int MAXOFMIN;
        public static Route lastRoute;

        static void Main(string[] args)
        {
            #region FILE로 Graph구성 -> DB로 대체예정
            FileStream fs = new FileStream("input.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string edge;

            string[] vertices = sr.ReadLine().Split(' ');

            Console.WriteLine("Graph 구성중..");
            Console.WriteLine("");

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

            Console.WriteLine("");
            Console.WriteLine("Graph 구성 완료!");
            Console.WriteLine("-----------------------------");

            #endregion FILE로 Graph구성
            // DB 호출 -> _dicGraphInfo -> List<Edge>

            #region Job 생성
            JOB job = new JOB("0", "7", MATERIAL_TYPE.DEFAULT_MATERIAL);
            #endregion

            #region BFS로 Routing
            { /* BFS하기 전에 초기화 */
                queue.Clear();
                isFound = false;
                MAXOFMIN = (int)DEFAULT_NUM.MINNUM;
                lastRoute = null;
            }
            // Start Vertex를 queue에 집어넣는다.
            queue.Enqueue(new Tuple<String, Route>(job.source, new Route(job.source)));

            Console.WriteLine("BFS 시작");
            Console.WriteLine("");

            BFS(job.terminal);
            #endregion BFS로 Routing

            #region Routing 결과
            Console.WriteLine("");
            Console.WriteLine("MaxOfMin(최소유량 중 최대값) = {0}", MAXOFMIN);
            Console.Write("가장 한가한 경로 : ");
            lastRoute.printRoute();
            #endregion Routing 결과

            #region 물류 객체 생성
            Material newMaterial;
            switch (job.MaterialType)
            {
                case MATERIAL_TYPE.DEFAULT_MATERIAL:
                    newMaterial = new DefaultMaterial(lastRoute);
                    break;
                default:
                    newMaterial = new DefaultMaterial(lastRoute);
                    break;
            }
            #endregion 물류 객체 생성

            #region Routing경로 사용
            Console.WriteLine("");
            Console.WriteLine("경로 사용..");
            try 
            {
                lastRoute.useRoute(g);
            }
            catch(RouteException re)
            {
                Console.WriteLine(re.Message);
            }
            finally
            {
                Console.WriteLine("경로 사용 완료!");
                Console.WriteLine("----------------------");

                Console.WriteLine("경로 설정된 후의 Graph");
                Console.WriteLine("");
                g.printGraph();
            }

            #endregion Routing경로 사용
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

                    Tuple<String, Route> nextTuple = new Tuple<String, Route>(e.to, newRoute);
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

    public class JOB
    {
        public string source { get; set; }
        public string terminal { get; set; }
        public MATERIAL_TYPE MaterialType { get; set; }
        
        public JOB(string s, string t, MATERIAL_TYPE mt)
        {
            source = s;
            terminal = t;
            MaterialType = mt;
        }
    }

    public enum DEFAULT_NUM
    {
        MAXNUM = 9999,
        MINNUM = -1
    }

    public enum MATERIAL_TYPE
    {
        DEFAULT_MATERIAL = 0
    }
}
