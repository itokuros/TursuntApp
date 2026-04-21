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
using Tursunt;

namespace WpfApp1
{
    public partial class AddEditPage : Page
    {
        private clusters _currentcluster = new clusters();

        public AddEditPage(clusters selectedclusters)
        {
            InitializeComponent();

            if (selectedclusters != null)
                _currentcluster = selectedclusters;

            DataContext = _currentcluster;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentcluster.cluster_name))
                errors.AppendLine("Укажите название кластера");
            if (string.IsNullOrWhiteSpace(_currentcluster.location))
                errors.AppendLine("Укажите локацию");
            if (string.IsNullOrWhiteSpace(_currentcluster.description))
                errors.AppendLine("Добавьте описание");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                using (var db = new НастяEntities()) // Создаем контекст здесь
                {
                    if (_currentcluster.cluster_id == 0)
                    {
                        // Добавление нового кластера
                        db.clusters.Add(_currentcluster);
                    }
                    else
                    {
                        // Обновление существующего
                        var cluster = db.clusters.Find(_currentcluster.cluster_id);
                        if (cluster != null)
                        {
                            cluster.cluster_name = _currentcluster.cluster_name;
                            cluster.location = _currentcluster.location;
                            cluster.description = _currentcluster.description;
                        }
                    }
                    db.SaveChanges();
                }
                MessageBox.Show("Информация сохранена!");

                // Возврат на предыдущую страницу
                if (NavigationService != null)
                    NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}