using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Task10.Interfaces;
using Task10.ViewModel;

namespace Task10.Model
{
    public class ModelItem : ViewModelBase, IModelItem
    {
        protected ObservableCollection<ModelItem> children;
        public ObservableCollection<ModelItem> ModelItems
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged("Children");
            }
        }

        protected string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("ParrentPath");
            }
        }

        protected string itemName;
        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        protected string fullPath;
        public string FullPath
        {
            get { return fullPath; }
            set
            {
                fullPath = value;
                OnPropertyChanged("Tag");
            }
        }

        protected BitmapSource image;
        public BitmapSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("PathToImage");
            }
        }

        protected string itemSize;
        public string ItemSize
        {
            get { return itemSize; }
            set
            {
                itemSize = value;
                OnPropertyChanged("ItemSize");
            }
        }

        protected bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        protected bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
