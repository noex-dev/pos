using Network;
using Server;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

namespace Client
{
    public partial class MainWindow : Window
    {
        readonly TcpClient client;
        readonly Transfer<MSG> transfer;

        public MainWindow()
        {
            InitializeComponent();

            client = new TcpClient("127.0.0.1", 12345);
            transfer = new Transfer<MSG>(client);

            transfer.OnMessageReceived += (sender, msg) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (msg.Type == MSG.MessageType.SEARCHRESULT)
                    {
                        foundNames.ItemsSource = msg.Names;
                    }

                    else if (msg.Type == MSG.MessageType.DETAILRESULT)
                    {
                        detailList.ItemsSource = msg.Details;
                        foundAlternatives.ItemsSource = msg.AlternativeNames;
                    }
                });
            };
        }

        private void SendQuery_Click(object sender, RoutedEventArgs e)
        {
            var msg = new MSG
            {
                Type = MSG.MessageType.SEARCH,
                Search = queryName.Text.ToString().Trim(),
                Sex = ((ComboBoxItem)querySex.SelectedItem).Content.ToString() == "Mann" ? "M" : "F"
            };

            transfer.Send(msg);
        }

        private void foundNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (foundNames.SelectedItem == null) { return; }

            var msg = new MSG();
            msg.Type = MSG.MessageType.DETAIL;
            msg.Sex = ((ComboBoxItem)querySex.SelectedItem).Content.ToString() == "Mann" ? "M" : "F";
            msg.DetailRequest = foundNames.SelectedItem.ToString();

            transfer.Send(msg);
        }

        private void foundAlternatives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (foundAlternatives.SelectedItem == null) { return; }

            var msg = new MSG();
            msg.Type = MSG.MessageType.DETAIL;
            msg.Sex = ((ComboBoxItem)querySex.SelectedItem).Content.ToString() == "Mann" ? "M" : "F";
            msg.DetailRequest = foundAlternatives.SelectedItem.ToString();

            transfer.Send(msg);
        }
    }
}