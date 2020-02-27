using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRS
{
    public class Graph
    {
        private static Graph _gInstance;

        private Dictionary<string, Vertex> _dicVertex = new Dictionary<string, Vertex>();

        public static Graph GetInstance()
        {
            if (_gInstance == null)
            {
                _gInstance = new Graph();
            }

            return _gInstance;
        }

        public void printGraph()
        {
            foreach (var key in _dicVertex.Keys)
            {
                Vertex v = _dicVertex[key];
                
                foreach (Edge e in v.getAdjList())
                {
                    Console.Write("Start Vertex : {0}, ", key);
                    e.printEdge();
                }
            }
        }

        public void addVertex(Vertex v, string key)
        {
            _dicVertex.Add(key, v);
        }

        public void addEdges(string from, string to, int capa)
        {
            _dicVertex.TryGetValue(from, out Vertex v);
            v.getAdjList().Add(new Edge(to, capa));
        }

        public Vertex getVertex(string key)
        {
            return _dicVertex[key];
        }
    }

    public class Vertex
    {
        private List<Edge> _adjList = new List<Edge>();

        public List<Edge> getAdjList()
        {
            return _adjList;
        }

        public List<Edge> getEdges()
        {
            return _adjList;
        }
    }

    public class Edge
    {
        private string to;
        private int capacity;
        private int usage = 0;
        private int weight;

        public Edge(string to, int capa)
        {
            this.to = to;
            this.capacity = capa;
            this.weight = capa;
        }

        public void printEdge()
        {
            Console.WriteLine("toVertex : {0}, Capacity : {1}, Weight : {2}", to, capacity, weight);
        }

        public int getCapa()
        {
            return capacity;
        }

        public string getTo()
        {
            return to;
        }

        public void EdgeUse()
        {
            usage++;
            weight = capacity - usage;
        }

        public void EdgeRecover()
        {
            usage--;
            weight = capacity - usage;
        }
    }


}
