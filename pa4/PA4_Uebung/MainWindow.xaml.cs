//using DataModels;
using DataModel;
using LinqToDB;
using PA4_Uebung.Parser;
using PA4_Uebung.Tokenizer;
using System.Text;
using System.Windows;
using Expression = PA4_Uebung.Parser.Expression;

namespace PA4_Uebung
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Code.Text = "COUNTRY Germany {\r\n  RANDOM\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY Austria {\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY France {\r\n  RANDOM\r\n  RANDOM\r\n  RANDOM\r\n}";

            // Aufgabe 7
            Code.Text = "COUNTRY Austria {\r\n  SELECT\r\n}\r\nCOUNTRY Germany {\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY France {\r\n  RANDOM\r\n  RANDOM\r\n  RANDOM\r\n}\r\nCOUNTRY Spain {\r\n  LARGEST\r\n  SMALLEST\r\n}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Expression.Errors.Clear();
            List<Token> tokens = new Lexer().Tokenize(Code.Text);

            var errors = tokens.Where(t => t.Type == Token.TokenType.Error).ToList();
            if (errors.Count > 0)
            {
                StringBuilder sb = new();
                sb.AppendLine("Fehlerhafte Tokens:");
                foreach (var error in errors)
                {
                    sb.AppendLine(error.Value);
                }
                MessageBox.Show(sb.ToString(), "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Program program = new();
            program.Parse(tokens);  

            if (Expression.Errors.Count > 0)
            {
                StringBuilder builder = new();
                builder.AppendLine("Fehlerhafte Anweisungen:");
                foreach (var error in Expression.Errors)
                {
                    builder.AppendLine(error);
                }
                MessageBox.Show(builder.ToString(), "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Don't run with invalid AST
            }

            // or just worldcities.sqlite
            using var db = new WorldCitiesDB(new DataOptions().UseSQLite(@"Data Source=C:\Users\noel\source\repos\PA4_Uebung\worldcities.sqlite"));
            List<Worldcity> allCities = [.. db.Worldcities];
            List<Worldcity> result = [];

            program.Run(allCities, result);

            StringBuilder output = new();
            foreach (var city in result)
                output.AppendLine($"{city.City} ({city.Country}) - {city.Population}");
            MessageBox.Show(output.Length > 0 ? output.ToString() : "Keine Ergebnisse", "Ergebnis");
        }
    }
}