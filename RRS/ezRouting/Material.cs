using ezRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezRouting
{
    public class Material
    {
        public Route MacroRoute;
        public Material(Route route)
        {
            MacroRoute = route;
        }
    }

    public class DefaultMaterial : Material
    {
        public DefaultMaterial(Route route) : base(route)
        {

        }
    }
}
