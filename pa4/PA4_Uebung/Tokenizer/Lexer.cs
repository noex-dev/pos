using System.CodeDom.Compiler;
using System.Text;
using System.Text.RegularExpressions;

namespace PA4_Uebung.Tokenizer
{
    public partial class Lexer
    {
        [GeneratedRegex(@"COUNTRY|SELECT|LARGEST|SMALLEST|RANDOM|{|}|\S+")]
        private static partial Regex TokenRegex();

        [GeneratedRegex(@"^[A-Za-z]+$")]
        private static partial Regex CountryRegex();

        private readonly Regex tokenRegex = TokenRegex();
        private readonly Regex countryRegex = CountryRegex();

        public List<Token> Tokenize(string code)
        {
            List<Token> tokens = [];
            foreach (Match match in tokenRegex.Matches(code))
            {
                Token.TokenType type = match.Value switch
                {
                    "COUNTRY" or "SELECT" or "LARGEST" or "SMALLEST" or "RANDOM" => Token.TokenType.Keyword,
                    "{" => Token.TokenType.OpenBracket,
                    "}" => Token.TokenType.CloseBracket,
                    _ when countryRegex.IsMatch(match.Value) => Token.TokenType.Country,
                    _ => Token.TokenType.Error
                };
                tokens.Add(new Token { Value = match.Value, Type = type });
            }
            return tokens;
        }

        public string TokenizeToString(string code)
        {
            StringBuilder sb = new();
            foreach (Token token in Tokenize(code))
            {
                sb.Append($"{token.Value} --> {token.Type}\n");
            }
            return sb.ToString();
        }
    }
}