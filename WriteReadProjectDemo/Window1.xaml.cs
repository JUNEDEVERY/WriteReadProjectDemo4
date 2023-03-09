using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace WriteReadProjectDemo
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<Product> products = new List<Product>();
        List<Article> articles = new List<Article>();

        User user;

        public Window1(User user)
        {
            InitializeComponent();
            this.user = user;
            //this.articleProducts = articleProducts;
            //List<string> articleProducts = new List<string>();

            cmbOrderPoint.ItemsSource = db.tbe.Point.ToList();
            cmbOrderPoint.SelectedValuePath = "idPickupPoint";
            cmbOrderPoint.DisplayMemberPath = "displayPoint";
            cmbOrderPoint.SelectedIndex = 0;
            foreach (Product product in db.tbe.Product.ToList())
            {

                foreach (string item in PageProducts.articleProducts)
                {

                    if (product.ProductArticleNumber == item)
                    {

                        products.Add(product);
                        Article article = new Article()
                        {
                            article = product.ProductArticleNumber,
                            count = 1,

                        };
                        articles.Add(article);

                    }
                }
            }
            lvOrder.ItemsSource = products;
            lvOrder.SelectedValuePath = "ProductArticleNumber";
            summOrder();
        }
        private void summOrder()
        {
            
            double sum = 0;
            double summWithDiscount = 0;
            foreach (var item in articles)
            {
                sum += Convert.ToDouble(db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.article).ProductCost * item.count);
                summWithDiscount += Convert.ToDouble(((db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.article).ProductCost - db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.article).ProductCost / 100 * db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.article).ProductDiscountAmount)) * item.count);
            }

            tbSaleZakaza.Text = "Общая скидка " + Convert.ToInt32((sum - summWithDiscount)).ToString() + "руб.";
            tbSummaZakaza.Text = "Итоговая цена " + Convert.ToInt32(Math.Round(summWithDiscount)).ToString() + "руб.";

        }
        private void dasd_Click(object sender, RoutedEventArgs e)
        {
            foreach (Product item in products)
            {
                MessageBox.Show(item.ProductArticleNumber);
            }

        }

        private void btnFormOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random random = new Random();
                int codeRND = random.Next(100, 999);
                Order order = new Order();
                order.OrderID = db.tbe.Order.Max(x => x.OrderID) + 1;
                order.OrderStatus = 1;
                order.OrderPickupPoint = (int)cmbOrderPoint.SelectedValue;
                order.OrderClientsId = user.UserID;
                order.Code = codeRND;
                order.OrderDate = DateTime.Now;

                foreach (Product item in products)
                {
                    if (item.ProductQuantityInStock < 3 || item.ProductQuantityInStock == 0)
                    {
                        order.OrderDeliveryDate = DateTime.Now.AddDays(6);
                    }
                    else
                    {
                        order.OrderDeliveryDate = DateTime.Now.AddDays(3);
                    }
                }

                db.tbe.Order.Add(order);
                db.tbe.SaveChanges();
                foreach (var item in articles)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.OrderID = order.OrderID;
                    orderProduct.ProductArticleNumber = item.article;
                    orderProduct.Count = item.count;
                    db.tbe.OrderProduct.Add(orderProduct);
                }

                db.tbe.SaveChanges();
                MessageBox.Show("ок");
                PageProducts.articleProducts.Clear();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void deleteMethod(string id)
        {
            
            PageProducts.articleProducts.Remove(id);
            if (PageProducts.articleProducts.Count == 0)
            {
                this.Close();
            }
            else
            {
                products.Clear();
                foreach (Product product in db.tbe.Product.ToList())
                {

                    foreach (string item in PageProducts.articleProducts)
                    {

                        if (product.ProductArticleNumber == item)
                        {

                            products.Add(product);
                            Article article = new Article()
                            {
                                article = product.ProductArticleNumber,
                                count = 1,

                            };
                            articles.Add(article);
                            MessageBox.Show(article.article);
                        }
                    }
                }
                lvOrder.Items.Refresh();
                lvOrder.ItemsSource = products;
                lvOrder.SelectedValuePath = "ProductArticleNumber";
                lvOrder.Items.Refresh();
                summOrder();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            string id = button.Uid;
            deleteMethod(id);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string id = textBox.Uid;
            articles.FirstOrDefault(x => x.article == id).count = Convert.ToInt32(textBox.Text);
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                summOrder();
            }
            if (textBox.Text.Equals("0"))
            {
                deleteMethod(id);
            }

        }
    }
}
