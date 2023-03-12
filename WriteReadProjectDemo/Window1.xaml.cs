using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
            orderSummaSale = Convert.ToInt32((sum - summWithDiscount)).ToString() + "руб.";
            orderSumma = Convert.ToInt32(Math.Round(summWithDiscount)).ToString() + "руб.";

        }

        public static string ordedDate;
        public static int orderID;
        public static string orderSostav;
        public static string orderSumma;
        public static string orderSummaSale;
        public static string orderPoint;
        // public static string orderPoint;

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
                if (user != null)
                {
                    order.OrderClientsId = user.UserID;
                }

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
                string orderPoint = cmbOrderPoint.Text;
                int orderID = order.OrderID;
                // формирование для смежной таблицы
                foreach (var item in articles)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.OrderID = order.OrderID;
                    orderProduct.ProductArticleNumber = item.article;
                    orderProduct.Count = item.count;
                    db.tbe.OrderProduct.Add(orderProduct);
                }
                string ordedDate = order.OrderDeliveryDate.ToString();
                db.tbe.SaveChanges();

                PageProducts.articleProducts.Clear();
                var ok = MessageBox.Show("Ваш заказ сформиован. Вам доступен талон для получения заказа. ", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);


                // формирование состава заказа
                string nameOrderProduct = "";
                List<OrderProduct> orderProductSostav = db.tbe.OrderProduct.Where(x => x.OrderID == order.OrderID).ToList();
                foreach (var item in orderProductSostav)
                {

                    Product product1 = db.tbe.Product.FirstOrDefault(x => x.ProductArticleNumber == item.ProductArticleNumber);

                    nameOrderProduct += product1.ProductName + $"({item.Count} шт.) ";


                }



                if (ok == MessageBoxResult.OK)
                {
                    pdfSharp(codeRND, ordedDate, nameOrderProduct, orderPoint, orderID, orderSumma, orderSummaSale);
                    this.Close();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void pdfSharp(int codeRND, string ordedDate, string nameOrderProduct, string orderPoint, int orderID, string orderSumma, string orderSummaSale)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF files(*.pdf)|*.pdf|All files(*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {

                // string txt = "Код для получения заказа: " + codeRND.ToString() + "\n" + "Дата заказа: \t" + ordedDate.ToString();
                PdfDocument pdf = new PdfDocument();
                pdf.Info.Title = "Талон для получения заказа";
                PdfPage pdfPage = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                XFont font = new XFont("Comic Sans MS", 20);
                XFont fontCode = new XFont("Comic Sans MS", 20, XFontStyle.Bold, options);

                string fileText = "Формирование талона получения для заказа " + orderID.ToString();
                gfx.DrawString("Дата заказа: " + ordedDate.ToString(), font, XBrushes.Black, new XRect(0, -355, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Номер заказа: " + orderID.ToString(), font, XBrushes.Black, new XRect(0, -320, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Состав заказа: " + nameOrderProduct.ToString(), font, XBrushes.Black, new XRect(0, -300, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Сумма заказа: " + orderSumma.ToString(), font, XBrushes.Black, new XRect(0, -255, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Сумма скидки: " + orderSummaSale.ToString(), font, XBrushes.Black, new XRect(0, -200, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Пункт выдачи: " + orderPoint.ToString(), font, XBrushes.Black, new XRect(0, -155, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                gfx.DrawString("Ваш код для получения заказа " + codeRND.ToString(), fontCode, XBrushes.Black, new XRect(0, -100, pdfPage.Width, pdfPage.Height), XStringFormat.Center);
                sfd.FileName = fileText;
                MessageBox.Show("Ваш заказ сформирован. Вам доступен талон для получения заказа. ", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                pdf.Save(sfd.FileName);

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
