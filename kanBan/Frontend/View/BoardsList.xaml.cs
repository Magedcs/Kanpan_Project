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
using Frontend.ViewModel;
using Frontend.Model;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardsList.xaml
    /// </summary>
    public partial class BoardsList : Window
    {
        private BoardsListViewModel viewModel;
        private UserModel user;
        public BoardsList(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardsListViewModel(u);
            this.DataContext = viewModel;
            this.user = u;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_Button(object sender, RoutedEventArgs e)
        {
            var c = viewModel.Getcords();
            BoardView view = new BoardView(user, c.name.Substring(13), int.Parse(c.id.Substring(11)));
            view.Show();
            this.Close();
        }
        private void Logout_Button(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
