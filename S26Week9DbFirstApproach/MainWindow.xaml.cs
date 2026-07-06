using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S26Week9DbFirstApproach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // instantiate the context class
        SchoolContext db = new SchoolContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadStudents()
        {
            var students = db.Students.ToList();
            grdStudents.ItemsSource = students;
        }

        private void btnLoadStudents_Click(object sender, RoutedEventArgs e)
        {
            LoadStudents();
        }
    }
}