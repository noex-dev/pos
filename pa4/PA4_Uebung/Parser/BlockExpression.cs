using DataModel;
using PA4_Uebung.Tokenizer;

namespace PA4_Uebung.Parser
{
    public class BlockExpression : Expression
    {
        private Program program = new Program();

        public override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.OpenBracket)
            {
                tokens.RemoveAt(0);
                program.Parse(tokens);

                if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.CloseBracket)
                {
                    tokens.RemoveAt(0);
                }
                else
                {
                    Errors.Add("Block: '}' erwartet");
                }
            }
            else
            {
                Errors.Add("Block: '{' erwartet");
            }
        }

        public override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            program.Run(cityList, resultList);
        }
    }
}
