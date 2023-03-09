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
        public double ActualPrice
        {
            get
            {

                if (ProductDiscountAmount != null)
                {

                    double ammount = Convert.ToDouble(ProductCost) - Convert.ToDouble(ProductCost) / 100 * Convert.ToDouble(ProductDiscountAmount);
                    return ammount;
                }
                else
                {
                    return Convert.ToDouble(ProductCost);
                }
              
            }
        }
        public SolidColorBrush colorBrush
        {
            get
            {
                if (ProductDiscountAmount > 15)
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(127, 255, 0));
                    return solidColorBrush;
                }
                else
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    return solidColorBrush;
                }
            }
        }
    }
}
