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
    /// Логика взаимодействия для PadeOrder.xaml
    /// </summary>
    public partial class PadeOrder : Page
    {
       public static  List<OrderProduct> orderProducts = db.tbe.OrderProduct.ToList();
        public PadeOrder()
        {
            InitializeComponent();

            cmbOrderPoint.ItemsSource = db.tbe.Point.ToList();
            cmbOrderPoint.SelectedValuePath = "idPickupPoint";
            cmbOrderPoint.DisplayMemberPath = "displayPoint";
            cmbOrderPoint.SelectedIndex = 0;
           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFormOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            if (textBlock.Uid != null)
            {
                textBlock.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
