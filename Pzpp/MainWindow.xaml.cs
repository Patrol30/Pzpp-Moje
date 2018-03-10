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
using MahApps.Metro.Controls;
using System.Data.Entity.Core.Objects;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace Pzpp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        

        
    
    public MainWindow()
        {
            //using (var db = new PingDataContext())
            //{
            //    db.Database.CreateIfNotExists();
            //}
            string dataPath = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain
                .CurrentDomain
                .SetData("DataDirectory", dataPath);
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

       

        

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    ComputersTable CT = new ComputersTable();
        //    CT.Show();
        //}

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    ResponsesTable RT = new ResponsesTable();
        //    RT.Show();
        //}
    }
}
