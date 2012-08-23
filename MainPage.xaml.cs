using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Fotoideen.Helper;
using Fotoideen.Model;
using Fotoideen.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;

namespace Fotoideen
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string VisibilityGroupName = "VisibilityStates";
        private const string OpenVisibilityStateName = "Open";
        private const string ClosedVisibilityStateName = "Closed";
        private const string StateKey_Value = "DateTimePickerPageBase_State_Value";


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (Content.SelectedItem == mainPan)
            {

            }
            else if (Content.SelectedItem == panWords)
            {
                Content.DefaultItem = Content.Items[Content.Items.IndexOf(mainPan)];
                e.Cancel = true;
            }
            else if (Content.SelectedItem == panSettings)
            {

            }

            base.OnBackKeyPress(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            State["index"] = Content.SelectedIndex;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (State.ContainsKey("index"))
            {
                Content.DefaultItem = Content.Items[int.Parse(State["index"].ToString())];
            }
            base.OnNavigatedTo(e);
        }

        private void Check()
        {

        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.Model;
            if (App.Model == null || App.Model.Items == null || App.Model.Items.Count == 0)
            {
                if (Content.Items.Contains(panWords))
                {
                    Content.Items.Remove(panWords);
                }
            }
        }

        private void SetRandomAmount()
        {
            var amount = App.RandomAmount;
            if (amount == 3)
            {
                dpRandom.SelectedIndex = 0;
            }
            else if (amount == 5)
            {
                dpRandom.SelectedIndex = 1;
            }
            else if (amount == 10)
            {
                dpRandom.SelectedIndex = 2;
            }
            else if (amount == 15)
            {
                dpRandom.SelectedIndex = 3;
            }
            else if (amount == 20)
            {
                dpRandom.SelectedIndex = 4;
            }
            else
            {
                dpRandom.SelectedIndex = 0;
            }

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Model.Items != null && App.Model.Items.Count > 1)
            {
                lbIdeas.SelectedIndex = 1;
            }
            dpRandom.SelectionChanged += new SelectionChangedEventHandler(dpRandom_SelectionChanged);
            SetRandomAmount();
        }


        private void StartUpdate()
        {
            ShowPogressIndicator();
            var proxy = new Proxy();
            proxy.ProxyEvent += new EventHandler(ProxyFinishedEvent);
            proxy.LoadData();
        }
        void ProxyFinishedEvent(object sender, EventArgs e)
        {
            var args = e as ProxyEventArgs;
            HandleProxy(args);
        }
        private void HandleProxy(ProxyEventArgs args)
        {
            if (!Deployment.Current.Dispatcher.CheckAccess())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => HandleProxy(args));
                return;
            }
            HidePogressIndicator();
            if (args.Code == HttpStatusCode.OK)
            {
                PushDataToModel(args.Response);
            }
            else if (args.Code == HttpStatusCode.Unused)
            {
                MessageBox.Show(Resource.message_Exception, Resource.hint, MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(Resource.message_WebExeption, Resource.hint, MessageBoxButton.OK);
            }

        }
        private void PushDataToModel(string normalizedJsonData)
        {
            var json = JsonConvert.DeserializeObject(normalizedJsonData) as Newtonsoft.Json.Linq.JObject;
            var table = json["table"];

            var rows = table.SelectToken("rows");
            int i = 0;
            if (App.Model.Items == null) App.Model.Items = new ObservableCollection<ColumnModel>();
            App.Model.Items.Clear();
            foreach (var row in rows.Children())
            {
                if (i == 0)
                {
                    var c = row.SelectToken("c");
                    foreach (var column in c)
                    {
                        App.Model.AddItem(column.Value<string>("v"));
                        App.Model.UpdateModel();
                    }
                }
                else
                {
                    var c = row.SelectToken("c");
                    int y = 0;
                    string name = string.Empty;
                    foreach (var column in c)
                    {
                        if (y == 0) // name
                        {
                            name = column.Value<string>("v");
                        }
                        else //belongs to
                        {
                            if (column.Value<string>("v") == "x")
                            {
                                App.Model.Items[y].AddItem(name);
                            }
                        }
                        y++;
                    }
                }
                i++;
            }

            if (App.Model != null && App.Model.Items != null && App.Model.Items.Count > 0)
            {
                if (!Content.Items.Contains(panWords))
                {
                    Content.Items.Insert(1, panWords);
                }
                panWords.DataContext = App.Model.Items[1];
            }


            //if (App.Model.Items != null && App.Model.Items.Count > 1)
            //{
            //    lbIdeas.SelectedIndex = 1;
            //}
        }

        private void lbWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox.SelectedIndex == -1) return;
            listbox.SelectedIndex = -1;
        }
        private void lbIdeas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedIndex == -1) return;
            var item = listBox.SelectedItem as ColumnModel;
            item.Update();
            panWords.DataContext = item;
            listBox.SelectedIndex = -1;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            StartUpdate();

        }


        private bool _isVisible = false;
        //private ProgressIndicator _indicator;
        private void ShowPogressIndicator()
        //private void ShowPogressIndicator(ProgressTypes type = ProgressTypes.WaitCursor, bool showLabel = true)
        {
            //if (!Deployment.Current.Dispatcher.CheckAccess())
            //{
            //    Deployment.Current.Dispatcher.BeginInvoke(() => ShowPogressIndicator(type, showLabel));
            //    return;
            //}
            //if (_isVisible) return;
            //if (_indicator == null) _indicator = new ProgressIndicator();
            //_indicator.ProgressType = type;
            //_indicator.ShowLabel = showLabel;
            //_indicator.Show();
            //_isVisible = true;
        }
        private void HidePogressIndicator()
        {
            //if (!Deployment.Current.Dispatcher.CheckAccess())
            //{
            //    Deployment.Current.Dispatcher.BeginInvoke(HidePogressIndicator);
            //    return;
            //}
            //if (!_isVisible) return;
            //if (_indicator == null) return;
            //_indicator.Hide();
            //_isVisible = false;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            App.Model.Reset();
            Content.DefaultItem = Content.Items[Content.Items.IndexOf(mainPan)];
            if (Content.Items.Contains(panWords))
            {
                Content.Items.Remove(panWords);
            }
        }

        private void dpRandom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as ListPicker;
            var amount = ((ListPickerItem)item.SelectedItem).Tag.ToString();
            App.RandomAmount = int.Parse(amount);
        }

        private void btnSheet_Click(object sender, RoutedEventArgs e)
        {
            var task = new WebBrowserTask();
            task.Uri = new Uri(Resource.linkSheet, UriKind.Absolute);
            task.Show();
        }
    }
}