using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteReadProjectDemo
{
    public partial class Point
    {
        public string displayPoint
        {
            get
            {
                string str = indexPoint.ToString() + " " + cityPoint + " " + streetPoint + " " + homePoint.ToString();
                return str;
            }
        }
    }
}
