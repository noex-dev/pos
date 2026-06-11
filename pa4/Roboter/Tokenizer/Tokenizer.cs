using System.Text;
using System.Text.RegularExpressions;

namespace Roboter.Tokenizer
{
    public partial class Tokenizer
    {
        [GeneratedRegex(@"REPEAT|UNTIL|MOVE|COLLECT|IF|IS-A|OBSTACLE|LEFT|RIGHT|UP|DOWN|\d+|[A-Z]|{|}|\S+")]
        private static partial Regex TokenRegex();

        private readonly Regex tokenRegex = TokenRegex();

        public List<Token> Tokenize(string code)
        {
            List<Token> tokens = [];
            foreach (Match match in tokenRegex.Matches(code))
            {
                Token.TokenType type = match.Value switch
                {
                    "REPEAT" or "UNTIL" or "IF" or "IS-A" or "MOVE" or "COLLECT" => Token.TokenType.Keyword,
                    "LEFT" or "RIGHT" or "UP" or "DOWN" => Token.TokenType.Direction,
                    "OBSTACLE" => Token.TokenType.Target,
                    "{" => Token.TokenType.OpenBracket,
                    "}" => Token.TokenType.CloseBracket,
                    _ when int.TryParse(match.Value, out _) => Token.TokenType.Number,
                    _ when match.Value.Length == 1 && char.IsLetter(match.Value[0]) => Token.TokenType.Letter,
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