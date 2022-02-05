using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Task10.Interfaces
{
    public interface IModelItem : INotifyPropertyChanged
    {
        string ItemName { get; set; }
        string FullPath { get; set; }
        string Status { get; set; }
        BitmapSource Image { get; set; }
        string ItemSize { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
    }
}
