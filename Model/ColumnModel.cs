using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

namespace Fotoideen.Model
{
    [DataContract]
    public class ColumnModel : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<CellModel> _items;
        private static Random Random = new Random(99999);

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; OnPropertyChanged("Name"); } }
        }
        [DataMember]
        public ObservableCollection<CellModel> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged("Items");
                    OnPropertyChanged("RandomItems");
                }
            }
        }


        public void AddItem(string name)
        {
            if (Items == null) Items = new ObservableCollection<CellModel>();
            if (!Items.Any(i => i.Name == name))
            {
                Items.Add(new CellModel() { Name = name });
            }
        }

        [IgnoreDataMember]
        public List<CellModel> RandomItems
        {
            get
            {
                var list = new List<CellModel>();
                var itemsCount = App.RandomAmount;
                if (Items.Count < itemsCount)
                {
                    itemsCount = Items.Count;
                }

                while (list.Count < itemsCount)
                {
                    var index = Random.Next(0, Items.IndexOf(Items.Last()));
                    var word = Items[index];
                    if (list.Contains(word))
                    {
                        continue;
                    }
                    list.Add(word);
                }
                return list;
            }
        }

        public void Update()
        {
            OnPropertyChanged("RandomItems");
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
