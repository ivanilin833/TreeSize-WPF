using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Task10.Model;

namespace Task10.ViewModel
{
    internal class MainVM
    {
        private readonly ViewItems viewItems;
        private ModelItem item;

        public MainVM(ViewItems _viewItems)
        {
            viewItems = _viewItems;
        }

        public IEnumerator<ModelItem> VerifyMatchingDirEnumerator(ModelItem model)
        {
            IEnumerable<ModelItem> matches = FindMatches(viewItems.SearchText.ToLower(), model);
            viewItems._matchingDirEnumerator = matches.GetEnumerator();

            if (!viewItems._matchingDirEnumerator.MoveNext())
            {
                MessageBox.Show(
                    "No matching names were found.",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
            return viewItems._matchingDirEnumerator;
        }

        public void GetItemFromPath(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles();

            Parallel.ForEach(diA, (df) => GetDir(df));
            Parallel.ForEach(fi, (f) => GetFile(f));
        }

        private void GetChildrenFile(FileInfo f, ObservableCollection<ModelItem> childrenItems)
        {
            try
            {
                childrenItems.Add(new ModelItem
                {
                    ItemName = GetFileOrFolderName(f.FullName),
                    FullPath = f.FullName,
                    Status = "success",
                    Image = GetImage(f.FullName),
                    ItemSize = ConvertSize(f.Length)
                });
            }
            catch
            {
                childrenItems.Add(new ModelItem
                {
                    ItemName = GetFileOrFolderName(f.FullName),
                    FullPath = f.FullName,
                    Image = GetImage(f.FullName),
                    Status = "Not available",
                    ItemSize = "-1"
                });
            }
        }

        private void GetFile(FileInfo f)
        {
            try
            {
                item = new ModelItem
                {
                    ItemName = GetFileOrFolderName(f.FullName),
                    FullPath = f.FullName,
                    Status = "success",
                    Image = GetImage(f.FullName),
                    ItemSize = ConvertSize(f.Length)
                };
                viewItems.AddItem(item);
            }
            catch
            {
                item = new ModelItem
                {
                    ItemName = GetFileOrFolderName(f.FullName),
                    FullPath = f.FullName,
                    Image = GetImage(f.FullName),
                    Status = "Not available",
                    ItemSize = "-1"
                };
                viewItems.AddItem(item);
            }
        }

        private void GetChildrenDir(DirectoryInfo df, ObservableCollection<ModelItem> childrenItems)
        {
            double catalogSize;
            catalogSize = 0;

            try
            {
                childrenItems.Add(new ModelItem
                {
                    ItemSize = ConvertSize(GetVolMem(df.FullName, ref catalogSize)),
                    ModelItems = GetChildrenItem(df.FullName),
                    Status = "success",
                    ItemName = GetFileOrFolderName(df.FullName),
                    FullPath = df.FullName,
                    Image = GetImage(df.FullName)
                });
            }
            catch
            {
                childrenItems.Add(item = new ModelItem
                {
                    ItemName = GetFileOrFolderName(df.FullName),
                    FullPath = df.FullName,
                    Image = GetImage(df.FullName),
                    Status = "Not available",
                    ItemSize = "-1"
                });
            }
        }

        private void GetDir(DirectoryInfo df)
        {
            double catalogSize;
            try
            {
                catalogSize = 0;
                item = new ModelItem
                {
                    ItemName = GetFileOrFolderName(df.FullName),
                    FullPath = df.FullName,
                    Status = "success",
                    Image = GetImage(df.FullName),
                    ModelItems = GetChildrenItem(df.FullName),
                    ItemSize = ConvertSize(GetVolMem(df.FullName, ref catalogSize)),
                };
                viewItems.AddItem(item);
            }
            catch
            {
                item = new ModelItem
                {
                    ItemName = GetFileOrFolderName(df.FullName),
                    FullPath = df.FullName,
                    Image = GetImage(df.FullName),
                    Status = "Not available",
                    ItemSize = "-1"
                };
                viewItems.AddItem(item);
            }
        }

        private ObservableCollection<ModelItem> GetChildrenItem(string path)
        {
            ObservableCollection<ModelItem> childrenItems = new ObservableCollection<ModelItem>();
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles();

            Parallel.ForEach(diA, (df) => GetChildrenDir(df, childrenItems));
            Parallel.ForEach(fi, (f) => GetChildrenFile(f, childrenItems));

            return childrenItems;
        }

        private string ConvertSize(double catalogSize)
        {
            string size = String.Empty;
            if (catalogSize < 8)
            {
                size = (catalogSize).ToString() + " байт";
            }
            else if (catalogSize > 8 && catalogSize < 1024)
            {
                size = Math.Round((catalogSize / 8), 2).ToString() + " бит";
            }
            else if (catalogSize > 1024 && catalogSize < 1048576)
            {
                size = Math.Round((catalogSize / 1024), 2).ToString() + " Кб";
            }
            else if (catalogSize > 1048576 && catalogSize < 1073741824)
            {
                size = Math.Round((catalogSize / 1048576), 2).ToString() + " Мб";
            }
            else
            {
                size = Math.Round((catalogSize / 1073741824), 2).ToString() + " Гб";
            }
            return size;
        }

        private double GetVolMem(string path, ref double catalogSize)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles();

                foreach (FileInfo f in fi)
                {

                    catalogSize += f.Length;
                }

                foreach (DirectoryInfo df in diA)
                {
                    GetVolMem(df.FullName, ref catalogSize);
                }

                return catalogSize;
            }

            catch
            {
                return 0;
            }
        }

        private string GetFileOrFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return (string.Empty);
            }

            string normalPath = path.Replace('/', '\\');
            int lastIndexName = normalPath.LastIndexOf('\\');

            return lastIndexName <= 0 ? path : path.Substring(lastIndexName + 1);
        }

        private BitmapSource GetImage(string path)
        {
            if (path == null)
            {
                return null;
            }

            string image = "Images/file.png";
            string nameAFile = GetFileOrFolderName(path);

            if (string.IsNullOrEmpty(nameAFile))
            {
                image = "Images/drive.png";
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                image = "Images/folder-closed.png";
            }

            var _image = new BitmapImage(new Uri($"pack://application:,,,/{image}"));
            _image.Freeze();

            return _image;
        }

        private IEnumerable<ModelItem> FindMatches(string searchText, ModelItem model)
        {
            if (model.ItemName.ToLower().Contains(searchText))
            {
                yield return model;
            }

            foreach (ModelItem child in model.ModelItems)
            {
                if (child.ModelItems == null)
                {
                    if (child.ItemName.ToLower().Contains(searchText))
                    {
                        model.IsExpanded = true;
                        yield return child;
                    }
                    continue;
                }

                foreach (ModelItem match in FindMatches(searchText, child))
                {
                    child.IsExpanded = true;
                    yield return match;
                }
            }
        } 
    }
}
