using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WriteReadProjectDemo
{
    public partial class Product
    {
        public SolidColorBrush colorBrushes
        {
            get
            {
                if (ProductQuantityInStock > 3)
                {

                    SolidColorBrush scb = (SolidColorBrush)new BrushConverter().ConvertFromString("#20b2aa");

                    return scb;



                }
                else
                {
                    SolidColorBrush scb1 = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff8c00");
                    return scb1;
                }
            }
        }
    }
}
