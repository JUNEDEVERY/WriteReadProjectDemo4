using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для Captcha.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        public static bool checkedCaptcha;
        int num = 0;
        public Captcha()
        {
            InitializeComponent();
            CreateImg();
        }
        private void CreateImg()
        {
            Random random = new Random();
            num = random.Next(1000, 9999);
            var pixels = new byte[Convert.ToInt32(CaptchaImage.Width) * Convert.ToInt32(CaptchaImage.Height) * 4];
            random.NextBytes(pixels);
            BitmapSource bitmapSource = BitmapSource.Create(Convert.ToInt32(CaptchaImage.Width), Convert.ToInt32(CaptchaImage.Height), 96, 96, PixelFormats.Bgra32, null, pixels, Convert.ToInt32(CaptchaImage.Width) * 4);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawText(
                    new FormattedText(num.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("Arial"), 100, System.Windows.Media.Brushes.Red), new System.Windows.Point(0, CaptchaImage.Height / 2));
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, 256, 256));
            }
            var image = new DrawingImage(visual.Drawing);
            CaptchaImage.Source = image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(num == Convert.ToInt32(tbCheckedCaptcha.Text))
            {
                if (!string.IsNullOrEmpty(tbCheckedCaptcha.Text))
                {
                    MessageBox.Show("Код введен верно.");
                    AuthorizationPage.checkedCaptcha = true;
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Введите код для капчи");
                }
                
                
            }
            else
            {
                MessageBox.Show("Код введен неверно.");

                AuthorizationPage.checkedCaptcha = false;
                this.Close();
            }
        }
    }
}
