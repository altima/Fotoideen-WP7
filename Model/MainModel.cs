using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Fotoideen.Helper;

namespace Fotoideen.Model
{
    [DataContract]
    public class MainModel : INotifyPropertyChanged
    {
        private const string ItemsStorageKey = "de.wp7dev.fotoideen.items";

        private ObservableCollection<ColumnModel> _items;
        [DataMember]
        public ObservableCollection<ColumnModel> Items
        {
            get { return _items; }
            set { if (_items != value) { _items = value; OnPropertyChanged("Items"); } }
        }

        public void AddItem(string name)
        {
            if (Items == null) Items = new ObservableCollection<ColumnModel>();
            Items.Add(new ColumnModel() { Name = name });
            OnPropertyChanged("Items");
        }

        [IgnoreDataMember]
        public MainModel Self
        {
            get { return this; }
        }

        public void UpdateModel()
        {
            OnPropertyChanged("Self");
        }

        public void Load()
        {
            try
            {
                Items = ApplicationSettings.GetSetting<ObservableCollection<ColumnModel>>(ItemsStorageKey);
            }
            catch (Exception ex)
            {
                Items = new ObservableCollection<ColumnModel>();
            }
        }
        public void Save()
        {
            if (Items == null) Items = new ObservableCollection<ColumnModel>();
            ApplicationSettings.SaveSetting(ItemsStorageKey, Items);
        }
        public void Reset()
        {
            Items.Clear();
            OnPropertyChanged("Self");
            OnPropertyChanged("Items");
            UpdateModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
