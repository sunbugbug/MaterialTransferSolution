using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            FileStream fs = new FileStream("input.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            //makeGraph(sr);

            makeGraphByDB();

            /* JOB 생성 확인을 위한 구문
            #endregion FILE로 Graph구성
            // DB 호출 -> _dicGraphInfo -> List<Edge>

            #region Job 생성
            /OB job = new JOB("0", "7", MATERIAL_TYPE.DEFAULT_MATERIAL);

            //StopWatch로 측정한 Routing 속도
            MeasureByStopWatch(job);

            //TickCounter로 측정한 Routing 속도
            MeasureByTickCounter(job);
            */
            
            return ;
        }

        static public void MeasureByStopWatch(JOB job)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            CreateMaterial(job);
            sw.Stop();
            Console.WriteLine("1개의 JOB을 Routing 하는데 {0}초 소요되었습니다.", (double)((double)sw.ElapsedMilliseconds / 1000));
        }

        static public void MeasureByTickCounter(JOB job)
        {
            long endCnt;
            long startCnt = 0;
            long frequency;

            QueryPerformanceCounter(out startCnt);
            CreateMaterial(job);
            QueryPerformanceFrequency(out frequency);
            QueryPerformanceCounter(out endCnt);

            double time = (endCnt - startCnt) / (double)frequency;

            Console.WriteLine("1개의 JOB을 Routing 하는데 {0}초 소요되었습니다.", time);
        }

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern bool QueryPerformanceCounter(out long counter);

        public static void CreateMaterial(JOB job)
        {
            #region BFS로 Routing
            { /* BFS하기 전에 초기화 */
                queue.Clear();
                isFound = false;
                MAXOFMIN = EnumClass.MINNUM;
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
            catch (RouteException re)
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
        }

        public static void makeGraphByDB()
        {

            Console.WriteLine("DB로 Graph 구성중...");
            Console.WriteLine("");


            using (StreamReader sr = new StreamReader(new FileStream("CIMTable.txt", FileMode.Open, FileAccess.Read)))
            { // Vertex 만들기
                string line = sr.ReadLine();
                string[] args = line.Split(' ');

                string CIMID;
                string CIMName;

                while ((line = sr.ReadLine()) != null)
                {
                    args = line.Split(' ');

                    CIMID = args[0];
                    CIMName = args[1];

                    Vertex v = new Vertex(CIMName);
                    g.addVertex(v, CIMID);
                }
            }

            using (StreamReader sr = new StreamReader(new FileStream("CIMEdgeTable.txt", FileMode.Open, FileAccess.Read)))
            { // Edge 생성(RRS용 Graph만 생성)
                string edge;

                string line = sr.ReadLine();
                string[] args = line.Split(' ');

                while ((edge = sr.ReadLine()) != null)
                {
                    args = edge.Split(' ');
                    string CIM1 = args[0], 
                        vCIM1 = args[1], 
                        CIM2 = args[2], 
                        vCIM2 = args[3], 
                        capa = args[4], 
                        doubleYN = args[5];

                    if (!CIM1.Equals(CIM2))
                    {
                        if (doubleYN.Equals("N"))
                        {
                            // 나중에 구현
                        }
                        else
                        {
                            g.addEdges(CIM1, CIM2, Convert.ToInt32(capa));
                        }
                    }
                }
            }
            g.printGraph();

            Console.WriteLine("");
            Console.WriteLine("Graph 구성 완료!");
            Console.WriteLine("-----------------------------");
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

            while ((edge = sr.ReadLine()) != null)
            {
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

                    if (e.weight <= 0) continue;
                    
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

}
