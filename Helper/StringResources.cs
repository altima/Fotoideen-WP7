using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Fotoideen.Resources;

namespace Fotoideen
{
    public class StringResources
    {
        private static Resource _resource = new Resource();
        public static Resource Stringresources
        {
            get { return _resource; }
        }
    }
}
