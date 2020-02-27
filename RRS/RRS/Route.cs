using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRS
{
    public class Route
    {
        List<string> _route = new List<string>();
        int minEdge = (int)DEFAULT_NUM.MAXNUM;

        public Route(String vertex)
        {
            _route.Add(vertex);
        }

        public Route(Route route)
        {
            this.minEdge = Math.Min(minEdge, route.getMinEdge());
            _route = route.copyRoute();
        }


        public void AddRoute(Edge e)
        {
            minEdge = Math.Min(minEdge, e.getCapa());
            _route.Add(e.getTo());
        }

        public List<string> getRoute()
        {
            return _route;
        }

        public List<string> copyRoute() // getRoute()로 Route를 받을 경우에 call By Reference가 되어 이전의 경로에 추가되는 식으로 적용되어 새로이 만든 함수
        {
            List<string> tempRoute = new List<string>();

            foreach(String s in _route)
            {
                tempRoute.Add(String.Copy(s));
            }

            return tempRoute;
        }

        public void printRoute()
        {
            foreach(String s in _route)
            {
                Console.Write("{0} -> ", s);
            }
            Console.WriteLine("minEdge = {0}", minEdge);
        }

        public int getMinEdge()
        {
            return minEdge;
        }

        public void useRoute()
        {
            for (int i = 0; i < _route.Count; i++)
            {
                string vName = _route[i];
                Vertex v = main.g.getVertex(vName);
                bool edgeUseFlag = false;

                if (i < _route.Count - 1)
                {
                    foreach (Edge e in v.getAdjList())
                    {
                        if (e.getTo().Equals(_route[i + 1])) { 
                            e.EdgeUse();
                            edgeUseFlag = true;
                        }
                    }
                    if (!edgeUseFlag)
                        throw new RouteException("Use Routed Edge Exception."); 
                    // Exception이 발생했을 경우 Exception 발생 이전의 경로를 다시 복구시켜줄 방법이 필요함.
                    // Exception 이전에 usage만 증가시켜놓고 useRoute가 끝난 후 weight에 반영하도록 하면 될 것 같음 -> 2번 호출해야될텐데 부하문제는 없을지 고민
                }
            }
        }
    }
}
