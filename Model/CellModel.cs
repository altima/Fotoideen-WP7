using System;
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

namespace Fotoideen.Model
{
    [DataContract]
    public class CellModel : INotifyPropertyChanged
    {
        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; OnPropertyChanged("Name"); } }
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
