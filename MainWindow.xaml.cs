using System;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using lab2_3.Data;
using lab2_3.Solvers;
using lab2_3.Solvers.Implementation;

namespace lab2_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Настройка фильтра файлов для удобства
            openFileDialog.Filter = "Все поддерживаемые файлы|*.txt;*.json;*.xml|Текстовые файлы (*.txt)|*.txt|JSON файлы (*.json)|*.json|XML файлы (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                PathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = PathTextBox.Text;

                if (string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show("Пожалуйста, выберите файл.");
                    return;
                }

                string extension = Path.GetExtension(path).ToLower();
                ParserBase parser = null;

                if (extension == ".txt")
                {
                    parser = new OscillatorParser(path);
                }
                else if (extension == ".json")
                {
                    parser = new RcParser(path);
                }
                else if (extension == ".xml")
                {
                    parser = new CoolingParser(path);
                }
                else
                {
                    throw new Exception("Неподдерживаемый формат файла.");
                }

                InputData data = parser.Parse();

                IEquationSolver solver = data.GetSolver();

                SolverResult result = solver.Solve(data);

                StatusLabel.Text = $"Успешно! {data.Name}: получено точек - {result.XValues.Length}";

                // Дополнительно можно вывести сообщение
                MessageBox.Show("Расчеты успешно завершены!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обработке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLabel.Text = "Ошибка выполнения.";
            }
        }
    }
}