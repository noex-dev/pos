using Roboter.Parser;
using System.Windows;

namespace Roboter
{
    public partial class MainWindow : Window
    {
        private readonly Tokenizer.Tokenizer tokenizer = new();
        private List<Tokenizer.Token> tokenList = [];

        public MainWindow()
        {
            InitializeComponent();

            Field.LoadField("Aufgabe2.xml");
            Code.Text = "REPEAT 2 {\r\n    MOVE RIGHT\r\n}\r\nREPEAT 6 {\r\n    MOVE DOWN\r\n}\r\nREPEAT 2 {\r\n    MOVE LEFT\r\n}\r\nCOLLECT\r\nREPEAT 4 {\r\n    MOVE RIGHT\r\n}\r\nMOVE DOWN\r\nCOLLECT\r\nMOVE RIGHT\r\nREPEAT 4 {\r\n    MOVE UP\r\n}\r\nMOVE LEFT\r\nCOLLECT";
        }

        private void Start_Program_Click(object sender, RoutedEventArgs e)
        {
            tokenList = tokenizer.Tokenize(Code.Text);
            TokensList.ItemsSource = tokenList;

            var program = new Program();
            Parser.Expression.Errors.Clear();
            program.Parse([.. tokenList]);

            if (Parser.Expression.Errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", Parser.Expression.Errors), "Error parsing the program");
                return;
            }

            Task.Run(() => program.Run(Field));
        }
    }
}