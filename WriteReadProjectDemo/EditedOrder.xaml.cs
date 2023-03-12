using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WriteReadProjectDemo
{
    /// <summary>
    /// Логика взаимодействия для EditedOrder.xaml
    /// </summary>
    public partial class EditedOrder : Page
    {



        List<Product> ORDERZ = new List<Product>();
        public static List<string> orders = new List<string>();

        public EditedOrder()
        {
            InitializeComponent();
            lvOrderEdited.ItemsSource = db.tbe.Order.ToList();





        }

        private void tbSostavZakaza_Loaded(object sender, RoutedEventArgs e)
        {
        }
        public static bool quantinyinStock;
        private void border_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;
            int id = Convert.ToInt32(border.Uid);
          
            List<OrderProduct> orderProduct = db.tbe.OrderProduct.Where(x => x.OrderID == id).ToList();
            foreach (var item in orderProduct)
            {

                Product product1 = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.ProductArticleNumber);
                if (item.Product.ProductQuantityInStock > 3)
                {
                    quantinyinStock = true;

                }
                else
                {
                    quantinyinStock = false;
                    break;

                }

            }
            if (quantinyinStock)
            {
                SolidColorBrush scb = (SolidColorBrush)new BrushConverter().ConvertFromString("#20b2aa");
                lvOrderEdited.Background = scb;
            }
            else
            {
                SolidColorBrush scb1 = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff8c00");
                lvOrderEdited.Background = scb1;
             
            }
        }
        private void tbSostavZakaza_Loaded_1(object sender, RoutedEventArgs e)
        {

            string nameOrderProduct = "";
            TextBlock textBlock = sender as TextBlock;
            int id = Convert.ToInt32(textBlock.Uid);
            List<OrderProduct> orderProduct = db.tbe.OrderProduct.Where(x => x.OrderID == id).ToList();
            foreach (var item in orderProduct)
            {

                Product product1 = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.ProductArticleNumber);

                nameOrderProduct += product1.ProductName + $"({item.Count} шт.) ";


            }
            textBlock.Text = nameOrderProduct;

        }

        private void tbSummZakaza_Loaded(object sender, RoutedEventArgs e)
        {

            int summa = 0;
            TextBlock textBlock = sender as TextBlock;
            int id = Convert.ToInt32(textBlock.Uid);
            List<OrderProduct> orderProduct = db.tbe.OrderProduct.Where(x => x.OrderID == id).ToList();
            foreach (var item in orderProduct)
            {

                Product product1 = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.ProductArticleNumber);

                summa += (int)product1.ProductCost * item.Count;


            }
            textBlock.Text = Convert.ToString(summa);
        }

        private void tbAllSale_Loaded(object sender, RoutedEventArgs e)
        {
            double summa = 0;
            double summa1 = 0;
            double procent = 0;
            TextBlock textBlock = sender as TextBlock;
            int id = Convert.ToInt32(textBlock.Uid);
            if (textBlock.Uid != null)
            {
                List<OrderProduct> orderProduct = db.tbe.OrderProduct.Where(x => x.OrderID == id).ToList();
                foreach (var item in orderProduct)
                {
                    Product product1 = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.ProductArticleNumber);
                    summa += (int)product1.ProductCost * item.Count; // просто цена

                    summa1 += (int)(product1.ProductCost - (product1.ProductCost * product1.ProductDiscountAmount / 100)) * item.Count; // с учетом скидки


                }
                procent = (summa - summa1) / (summa / 100);
                textBlock.Text = summa1.ToString() + $"({procent} %) ";
            }
            else
            {
                textBlock.Text = "";
            }
        }

        private void lvOrderEdited_Loaded(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            string id = listView.Uid;
            Product product = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == id);
            //var color = (Color)ColorConverter.ConvertFromString("#20b2aa");
            foreach (var item in listView.ItemsSource)
            {
                SolidColorBrush scb = (SolidColorBrush)new BrushConverter().ConvertFromString("#20b2aa");
                if (product.ProductQuantityInStock > 3)
                {

                    lvOrderEdited.Background = scb;
                }
                else if (product.ProductQuantityInStock == 0)
                {
                    SolidColorBrush scb1 = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff8c00");
                    lvOrderEdited.Background = scb1;
                }
                else
                {
                    lvOrderEdited.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }

        }

        private void lvOrderEdited_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

    }
}
