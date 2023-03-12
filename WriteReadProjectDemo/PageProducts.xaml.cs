using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
    /// Логика взаимодействия для PageProducts.xaml
    /// </summary>
    public partial class PageProducts : Page
    {
         User user;
        public static bool isGuest;
      
        Product product1 = new Product();
        public PageProducts(User user)
        {
            InitializeComponent();
            this.user = user;
            db.tbe = new Entities();
            lvProduct.ItemsSource = db.tbe.Product.ToList();
            lvProduct.SelectedValuePath = "ProductArticleNumber";
            tbFirst.Text = lvProduct.Items.Count.ToString();
            tblast.Text = db.tbe.Product.Count().ToString();
            tbNameUser.Text = user.FullName;
            isGuest = false;

        }
        public PageProducts() // для гостя
        {
            InitializeComponent();
            db.tbe = new Entities();
            lvProduct.ItemsSource = db.tbe.Product.ToList();
            lvProduct.SelectedValuePath = "ProductArticleNumber";
            tbFirst.Text = lvProduct.Items.Count.ToString();
            tblast.Text = db.tbe.Product.Count().ToString();
            tbNameUser.Text = "Гость";
            isGuest = true;

        }
        private void cmbSorted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filtresMethod();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtresMethod();
        }

        private void lvProduct_Loaded(object sender, RoutedEventArgs e)
        {
            //List<Product> product = db.tbe.Product.Where(x => x.ProductDiscountAmount > 15).ToList();
            //if (product1.ProductDiscountAmount > 15)
            //{
            //    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(127, 255, 0));
            //    return solidColorBrush;
            //}
        }
        private void filtresMethod()
        {
            List<Product> products = db.tbe.Product.ToList();
            if (cmbSorted.SelectedItem != null)
            {
                ComboBoxItem comboBoxItem = (ComboBoxItem)cmbSorted.SelectedItem;
                switch (comboBoxItem.Content)
                {

                    case "По умолчанию":
                        {
                            products = products;
                            break;
                        }
                    case "По возрастанию стоимости":
                        {
                            products = products.OrderBy(x => x.ProductCost).ToList();
                            break;
                        }
                    case "По убыванию стоимости":
                        {

                            products = products.OrderByDescending(x => x.ProductCost).ToList();
                            break;
                        }
                }


            }
            if (cmbFiltres.SelectedItem != null)
            {
                ComboBoxItem comboBoxItem = (ComboBoxItem)cmbFiltres.SelectedItem;
                switch (comboBoxItem.Content)
                {

                    case "Все диапазоны":
                        {
                            products = products;
                            break;
                        }

                    case "0-9,99%":
                        {
                            products = products.Where(x => x.ProductDiscountAmount >= 0 && x.ProductDiscountAmount <= 9.99).ToList();
                            break;
                        }
                    case "10-14,99%":
                        {
                            products = products.Where(x => x.ProductDiscountAmount >= 10 && x.ProductDiscountAmount <= 14.99).ToList();

                            break;
                        }
                    case "15% и более":
                        {
                            products = products.Where(x => x.ProductDiscountAmount >= 15 ).ToList();

                            break;
                        }

                }
            }
            if(tbSearch.Text != null)
            {
                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    products = products.Where(x => x.ProductName.ToLower().Contains(tbSearch.Text)).ToList();
                }
            }

       
            lvProduct.ItemsSource = products;
            tblast.Text = lvProduct.Items.Count.ToString();
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

        private void tbFirst_Loaded(object sender, RoutedEventArgs e)
        {

            List<Product> products = db.tbe.Product.ToList();
            tbFirst.Text = db.tbe.Product.ToList().Count().ToString();
            lvProduct.ItemsSource=products;
        }

        private void tblast_Loaded(object sender, RoutedEventArgs e)
        {

            List<Product> products = db.tbe.Product.ToList();
            tblast.Text = lvProduct.Items.Count.ToString();
            lvProduct.ItemsSource = products;
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1(user);
            window.Show();
            window.Closing += (obj, args) =>
            {
                if (articleProducts.Count == 0)
                {
                    btnOrder.Visibility = Visibility.Collapsed;

                }
                else
                {
                    btnOrder.Visibility = Visibility.Visible;
                }
            };
            
        }
        public static List<string> articleProducts = new List<string>();
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            btnOrder.Visibility = Visibility.Visible;
            articleProducts.Add(lvProduct.SelectedValue.ToString());
           
        }

        private void tbSale_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            if(textBlock.Uid != null)
            {
                textBlock.Visibility = Visibility.Visible;

            }
            else
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string id = button.Uid;
            Product product = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == id);
            

        }

        private void btnCHangeOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditedOrder());
        }
    }
}
