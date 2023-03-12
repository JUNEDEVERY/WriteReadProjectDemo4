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
using System.Windows.Shapes;

namespace WriteReadProjectDemo
{
    /// <summary>
    /// Логика взаимодействия для WindowChangeOrderADM.xaml
    /// </summary>
    public partial class WindowChangeOrderADM : Window
    {
        Order order;
        public WindowChangeOrderADM(Order order)
        {
            InitializeComponent();
            this.order = order;
           
            datePicker.SelectedDate = order.OrderDate;
            cmbStatus.Text = order.OrderStatus.ToString();
            cmbStatus.ItemsSource = db.tbe.statusChange.ToList();
            cmbStatus.SelectedValuePath = "idStatus";
            cmbStatus.DisplayMemberPath = "statusName";
            cmbStatus.SelectedIndex = 0;



        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
           
                if (cmbStatus != null)
                {
                    if (cmbStatus.SelectedIndex != -1)
                    {
                        if (datePicker != null)
                        {
                            if (datePicker.SelectedDate != null)
                            {
                               
                                DateTime dt = datePicker.SelectedDate.Value;
                           
                                order.OrderStatus = Convert.ToInt32(cmbStatus.SelectedValue);
                                order.OrderDate = dt; 
                                db.tbe.SaveChanges();
                            MessageBox.Show("ок");
                            this.Close();
                            }

                        }
                    }
                }
          
          
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
