using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tursunt;

namespace WpfApp1
{
    public partial class HIPage : Page
    {
        public HIPage()
        {
            InitializeComponent();
            LoadData();
            LoadLocationFilter();
        }

        private void LoadData()
        {
            using (var db = new НастяEntities())
            {
                var query = db.clusters.AsQueryable();

                // Поиск
                if (!string.IsNullOrWhiteSpace(tbSearch.Text))
                {
                    string search = tbSearch.Text.ToLower();
                    query = query.Where(c => c.cluster_name.ToLower().Contains(search) ||
                                              c.location.ToLower().Contains(search));
                }

                // Фильтр по локации
                if (cbLocationFilter.SelectedItem != null && cbLocationFilter.SelectedIndex > 0)
                {
                    string selectedLocation = cbLocationFilter.SelectedItem.ToString();
                    query = query.Where(c => c.location == selectedLocation);
                }

                DGridHi.ItemsSource = query.ToList();
            }
        }

        private void LoadLocationFilter()
        {
            using (var db = new НастяEntities())
            {
                var locations = db.clusters.Select(c => c.location).Distinct().ToList();
                locations.Insert(0, "Все");
                cbLocationFilter.ItemsSource = locations;
                cbLocationFilter.SelectedIndex = 0;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        private void cbLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cluster = button?.DataContext as clusters;
            if (cluster != null)
            {
                Manager.MainFrame.Navigate(new AddEditPage(cluster));
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridHi.SelectedItem as clusters;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления!");
                return;
            }

            var result = MessageBox.Show($"Удалить кластер '{selected.cluster_name}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new НастяEntities())
                    {
                        var cluster = db.clusters.Find(selected.cluster_id);
                        if (cluster != null)
                        {
                            db.clusters.Remove(cluster);
                            db.SaveChanges();
                        }
                    }
                    LoadData();
                    MessageBox.Show("Запись удалена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LoadData();
                LoadLocationFilter();
            }
        }
    }
}