using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Dimebrain.TweetSharp;
using Dimebrain.TweetSharp.Model;
using Dimebrain.TweetSharp.Extensions;
using Dimebrain.TweetSharp.Fluent;
using System.Xml.Linq;
using TakeOrdersOverTwitterWPF.Properties;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Diagnostics;
using System.Xml;

namespace TakeOrdersOverTwitterWPF
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void applySettings_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings Saved!","Tweet Sandwich");
            CheckForOrders(null, null);
        }

        DispatcherTimer twitterTimer = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.twitterTimer = new DispatcherTimer(new TimeSpan(0, 5, 0), DispatcherPriority.Normal, CheckForOrders, this.Dispatcher);
        }

        private void CheckForOrders(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Password.Password) == false)
            {
                long lastOrderNum = Settings.Default.LastOrder;
                
                //Twitter Bug, t'aint me.
                if (lastOrderNum == 0) lastOrderNum = 1;
                
                TwitterClientInfo info = new TwitterClientInfo() { ClientName = "TweetSandwich", ClientVersion = "1.0" };
                var replies = FluentTwitter.CreateRequest(info)
                        .AuthenticateAs(twitterUserName.Text, Password.Password)
                        .Statuses()
                        .Replies()
                        .Since(lastOrderNum)
                        .AsJson();
                LogMessage(String.Format("Making request: {0}", replies.ToString()));

                IEnumerable<TwitterStatus> statuses = replies.Request().AsStatuses();
                if (statuses != null)
                {
                    var statusesFiltered = from d in statuses
                                           where d.Id > lastOrderNum
                                           where d.Text.IndexOf(orderString.Text, StringComparison.OrdinalIgnoreCase) != -1
                                           orderby d.CreatedDate descending
                                           select d;

                    //XDocument doc = XDocument.Parse(result);
                    //var statuses = (from d in doc.Descendants("status")
                    //                where int.Parse(d.Element("id").Value) > lastOrderNum
                    //                where d.Element("text").Value.Contains(orderString.Text)
                    //                select new
                    //                {
                    //                    tweetid = int.Parse(d.Element("id").Value),
                    //                    name = d.Element("user").Element("name").Value,
                    //                    location = d.Element("user").Element("location").Value,
                    //                    tweet = d.Element("text").Value,
                    //                    dateTime = d.Element("created_at").Value.ParseTwitterDateTime()
                    //                }).OrderByDescending(t => t.dateTime);

                    if (statusesFiltered.Count() > 0)
                    {
                        Settings.Default.LastOrder = statusesFiltered.Max(i => i.Id);
                        Settings.Default.Save();
                        System.Media.SystemSounds.Exclamation.Play();
                        PrintOrders(statusesFiltered);
                    }

                }
                else
                {
                    LogMessage("No orders.");
                }

            }
        }

        public void LogMessage(string msg)
        {
            listBox1.Items.Insert(0,DateTime.Now.ToString() + ": " + msg);
        }

        private void PrintOrders(IEnumerable<TwitterStatus> statuses)
        {
            foreach(TwitterStatus t in statuses)
            {
                LogMessage(String.Format("Found order from {0} from {1}.", t.User.Name, t.User.Location));

                UpdateOrderCanvas(t);

                PrintOrder(t);

                //PrintDialog dlg = new PrintDialog();
                //dlg.PrintVisual(orderCanvas, "Tweeted Order");
            }
            //throw new NotImplementedException();
        }

        private void PrintOrder(TwitterStatus t)
        {
            var streamInfo = Application.GetResourceStream(new Uri("resources/SandwichOrder.xaml",UriKind.Relative));
            FlowDocument doc = XamlReader.Load(streamInfo.Stream) as FlowDocument;
            doc.DataContext = t;
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.SystemIdle, new DispatcherOperationCallback(delegate { return null; }), null);
            PrintDialog dlg = new PrintDialog();
            dlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator,"Tweeted Sandwich Order");
        }

        private void UpdateOrderCanvas(TwitterStatus t)
        {
            orderAvatar.Source = new BitmapImage(new Uri(t.User.ProfileImageUrl));
            orderDateTime.Content = t.CreatedDate;
            orderLocation.Content = t.User.Location;
            orderName.Content = t.User.Name;
            orderTwitterName.Content = t.User.ScreenName;
            orderTweet.Text = t.Text;

        }

    }

    //public static class StringExtensions
    //{
    //    public static DateTime ParseTwitterDateTime(this string date)
    //    {
    //        string dayOfWeek = date.Substring(0, 3).Trim();
    //        string month = date.Substring(4, 3).Trim();
    //        string dayInMonth = date.Substring(8, 2).Trim();
    //        string time = date.Substring(11, 9).Trim();
    //        string offset = date.Substring(20, 5).Trim();
    //        string year = date.Substring(25, 5).Trim();
    //        string dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
    //        DateTime ret = DateTime.Parse(dateTime);
    //        return ret;
    //    }
    //}

    namespace BindableText
    {
        /// <summary>
        /// A subclass of the Run element that exposes a DependencyProperty property
        /// to allow data binding.
        /// </summary>
        public class BindableRun : Run
        {
            public static readonly DependencyProperty BoundTextProperty = DependencyProperty.Register("BoundText", typeof(string), typeof(BindableRun), new PropertyMetadata(new PropertyChangedCallback(BindableRun.onBoundTextChanged)));

            private static void onBoundTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                ((Run)d).Text = (string)e.NewValue;
            }

            public String BoundText
            {
                get { return (string)GetValue(BoundTextProperty); }
                set { SetValue(BoundTextProperty, value); }
            }
        }
    }
}
