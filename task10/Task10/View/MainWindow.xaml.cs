using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Task10.Model;
using Task10.ViewModel;

namespace Task10
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ViewItems _model;
        private MainVM start;

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFileDlg = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            _model = new ViewItems();
            start = new MainVM(_model);
            string choiceFolder;

            if (openFileDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                choiceFolder = openFileDlg.FileName;
                ChoiceFolder.Text = choiceFolder;
                DataContext = _model;

                MessageBox.Show(
                    "Scanning started .",
                    "Scan",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );

                start.GetItemFromPath(choiceFolder);
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            ModelItem item;

            ModelItem model = new ModelItem
            {
                ItemName = "ItemNameToStartSearch",
                ModelItems = (System.Collections.ObjectModel.ObservableCollection<ModelItem>)TreeList.ItemsSource
            };

            if (_model._matchingDirEnumerator == null || !_model._matchingDirEnumerator.MoveNext())
            {
                _model._matchingDirEnumerator = start.VerifyMatchingDirEnumerator(model);
            }

            item = _model._matchingDirEnumerator.Current;

            if (item == null)
            {
                return;
            }

            item.IsSelected = true;
        }
    }
}
