using AbcRobotCore;
using System.CodeDom;

namespace Roboter.Parser
{
    public class Program : Expression
    {
        private readonly List<Expression> expressions = [];

        public override void Parse(List<Tokenizer.Token> tokens)
        {
            while (tokens.Count > 0 && tokens[0].Type != Tokenizer.Token.TokenType.CloseBracket)
            {
                Tokenizer.Token token = tokens[0];
                if (token.Type == Tokenizer.Token.TokenType.Keyword)
                {
                    Expression expression;
                    switch (token.Value)
                    {
                        case "MOVE":
                            expression = new MoveExpression(); break;

                        case "REPEAT":
                            expression = new RepeatExpression(); break;

                        case "UNTIL":
                            expression = new UntilExpression(); break;

                        case "IF":
                            expression = new IfExpression(); break;

                        case "COLLECT":
                            expression = new CollectExpression(); break;

                        default:
                            Errors.Add($"Unknown Keyword: {token.Value}"); tokens.RemoveAt(0); continue;
                    }

                    tokens.RemoveAt(0);
                    expression.Parse(tokens);
                    expressions.Add(expression);
                }

                else
                {
                    Errors.Add($"Unexpected token type: {token.Type}");
                    tokens.RemoveAt(0);
                }
            }
        }

        public override void Run(RobotField robot)
        {
            foreach (var expression in expressions)
            {
                expression.Run(robot);
            }
        }
    }
}
