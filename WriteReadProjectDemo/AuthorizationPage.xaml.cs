using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace WriteReadProjectDemo
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static bool UserRole;
        private DispatcherTimer dispatcher;
        public static bool checkedCaptcha;
        private int counter = 10;
        public AuthorizationPage()
        {
            InitializeComponent();
            dispatcher = new DispatcherTimer();
            dispatcher.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcher.Tick += new EventHandler(TimerEnd);
         


        }
      
        private void TimerEnd(object sender, EventArgs e)
        {
            try
            {
              
                if (counter != 0)
                {
                    
                    tbAuth.Text = "Новый код доступен через \n\t" + string.Format("00:00:{0}", counter) + " секунд ";


                }
                else
                {
                    btnAuth.IsEnabled = true;
                    gpPassword.IsEnabled = true;
                    gpLogin.IsEnabled = true;
                   
                    tbAuth.Visibility = Visibility.Collapsed;
                    dispatcher.Stop();
                }
                counter--;

            }
            catch
            {
                MessageBox.Show("Дваайте еще раз попробуем");
            }



        }
  
        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            User user = db.tbe.User.FirstOrDefault(x => x.UserPassword == tbPassword.Text && x.UserLogin == tbLogin.Text);
            try
            {
                if (user == null)
                {
                    //tbLogin.Text != user.UserLogin && tbPassword.Text != user.UserPassword
                    MessageBox.Show("Введенный логин и/или пароль неверен");
                    Captcha captcha = new Captcha();
                    captcha.Show();
                    captcha.Closing += (obj, args) =>
                    {
                        if (!checkedCaptcha)
                        {
                            btnAuth.IsEnabled = false;
                            gpPassword.IsEnabled = false;
                            gpLogin.IsEnabled = false;
                            counter = 10;
                            dispatcher.Start();
                        }
                        
                    };

                }
                else
                {
                    if (!string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrEmpty(tbPassword.Text))
                    {
                        if (tbLogin.Text != null && tbPassword.Text != null)
                        {


                            if (user.UserLogin == tbLogin.Text)
                            {
                                if (user.UserPassword == tbPassword.Text)
                                {
                                    if (user.UserRole == 1) // админ
                                    {
                                        NavigationService.Navigate(new PageProducts(user));
                                    }
                                    else if (user.UserRole == 2) // менеджер
                                    {

                                    }
                                    else if (user.UserRole == 3) // клиент
                                    {
                                        NavigationService.Navigate(new PageProducts(user));
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Пароль в системе отсутствует");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Логин в системе отсутствует");
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Неверный логин и/или пароль!");
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void goGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageProducts());
        }
    }
}
