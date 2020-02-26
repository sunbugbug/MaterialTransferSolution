using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRS
{
    public class Route
    {
        LinkedList<String> _route = new LinkedList<String>();
        int minEdge = (int)DEFAULT_NUM.MAXNUM;

        public Route(String vertex)
        {
            _route.AddLast(vertex);
        }

        public Route(Route route)
        {
            this.minEdge = Math.Min(minEdge, route.getMinEdge());
            this._route = route.copyRoute();
        }


        public void AddRoute(Edge e)
        {
            minEdge = Math.Min(minEdge, e.getCapa());
            _route.AddLast(e.getTo());
        }

        public LinkedList<String> getRoute()
        {
            return _route;
        }

        public LinkedList<String> copyRoute() // getRoute()로 Route를 받을 경우에 call By Reference가 되어 이전의 경로에 추가되는 식으로 적용되어 새로이 만든 함수
        {
            LinkedList<String> tempRoute = new LinkedList<String>();

            foreach(String s in _route)
            {
                tempRoute.AddLast(String.Copy(s));
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
    }
}
