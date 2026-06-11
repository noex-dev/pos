using DataModel;
using LinqToDB;
using Microsoft.Data.Sqlite;
using Network;
using Server;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Serialization;
using NinjaNye.SearchExtensions.Soundex;

internal class Programm
{
    internal Programm()
    {
        InitOrmWrapper(true);
        SetupServer();
    }

    private static BabynamesDb InitOrmWrapper(bool printMessage = false)
    {
        var options = new DataOptions().UseSQLite("Data Source=Babynames.db");
        var db = new BabynamesDb(new DataOptions<BabynamesDb>(options));

        if (!printMessage) { return db; }

        if (db.Babynames.FirstOrDefault() != null)
        {
            Console.WriteLine("ORM-Wrapper works!");
        }

        else
        {
            Console.WriteLine("ORM-Wrapper does not work!");
        }

        return db;
    }

    private void SetupServer()
    {
        var tcpListener = new TcpListener(IPAddress.Any, 12345);
        tcpListener.Start();

        while (true)
        {
            var client = tcpListener.AcceptTcpClient();
            new Thread(() => HandleClient(client)).Start();
        }
    }

    private void HandleClient(TcpClient client)
    {
        Console.WriteLine("Client connected");

        var db = InitOrmWrapper();
        var transfer = new Transfer<MSG>(client);

        transfer.OnMessageReceived += (sender, msg) =>
        {
            if (msg == null)
            {
                return;
            }

            Console.WriteLine("Message received");
            if (msg.Type == MSG.MessageType.SEARCH)
            {
                var names = db.Babynames
                        .Where(b => b.Name.Contains(msg.Search, StringComparison.OrdinalIgnoreCase) && b.Sex == msg.Sex)
                        .Select(b => b.Name)
                        .Distinct()
                        .ToList();

                var returnMsg = new MSG();
                returnMsg.Type = MSG.MessageType.SEARCHRESULT;
                returnMsg.Names = names;
                transfer.Send(returnMsg);
            }

            else if (msg.Type == MSG.MessageType.DETAIL)
            {
                var details = db.Babynames.Where(b => b.Name == msg.DetailRequest && b.Sex == msg.Sex);
                var alternatives = db.Babynames.SoundexOf(b => b.Name)
                                    .Matching(msg.DetailRequest)
                                    .Where(b => b.Sex == msg.Sex)
                                    .Where(b => b.Name != msg.DetailRequest)
                                    .Select(b => b.Name)
                                    .Distinct()
                                    .ToList();

                var resultMsg = new MSG();
                resultMsg.Type = MSG.MessageType.DETAILRESULT;
                resultMsg.Details = details.Select(b => new BabynameDetail
                {
                    Year = b.Year,
                    Count = b.Count
                }).ToList();
                resultMsg.AlternativeNames = alternatives;
                transfer.Send(resultMsg);
            }
        };

        transfer.OnLineReceived += (sender, e) =>
        {
            Console.WriteLine("Line received");
        };

        transfer.OnDisconnected += (sender, e) =>
        {
            Console.WriteLine("Client disconnected");
        };
    }

    public static void Main(string[] args)
    {
        _ = new Programm();

        Console.ReadLine();
    }
}