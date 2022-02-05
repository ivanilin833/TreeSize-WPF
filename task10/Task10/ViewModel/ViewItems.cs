using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Task10.Model;

namespace Task10.ViewModel
{
    internal class ViewItems : ViewModelBase
    {
        protected ObservableCollection<ModelItem> _modelItems;
        private string _searchText = String.Empty;
        public IEnumerator<ModelItem> _matchingDirEnumerator;

        public ViewItems()
        {
            _modelItems = new ObservableCollection<ModelItem>();
        }

        public ObservableCollection<ModelItem> ViewItem => _modelItems;

        public string SearchText
        {
            get { return _searchText; }
            set
            {

                if (value == _searchText)
                    return;

                _searchText = value;
                _matchingDirEnumerator = null;
            }
        }

        public void AddItem(ModelItem item)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                ViewItem.Add(item);
            }

            else
            {
                addItem add = AddItem;
                Application.Current.Dispatcher.BeginInvoke(add, item);
            }
        }
        private delegate void addItem(ModelItem item);
    }
}
