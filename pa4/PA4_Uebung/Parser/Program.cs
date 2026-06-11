using DataModel;
using PA4_Uebung.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Uebung.Parser
{
    public class Program : Expression
    {
        private readonly List<Expression> expressions = [];

        public override void Parse(List<Token> tokens)
        {
            while (tokens.Count > 0 && tokens[0].Type != Token.TokenType.CloseBracket)
            {
                Token token = tokens[0];

                if (token.Type == Token.TokenType.Keyword)
                {
                    Expression expression;
                    switch (token.Value)
                    {
                        case "COUNTRY":
                            expression = new CountryExpression(); break;
                        case "SELECT":
                            expression = new SelectExpression(); break;
                        case "RANDOM":
                            expression = new RandomExpression(); break;
                        case "LARGEST":
                            expression = new LargestExpression(); break;
                        case "SMALLEST":
                            expression = new SmallestExpression(); break;
                        default:
                            Errors.Add($"Program: Unexpected Keyword {token.Value}");
                            tokens.RemoveAt(0);
                            continue;
                    }
                    tokens.RemoveAt(0);
                    expression.Parse(tokens);
                    expressions.Add(expression);
                }

                else
                {
                    Errors.Add($"Unexpected Token Type {token.Type}");
                    tokens.RemoveAt(0);
                }
            }
        }

        public override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            foreach (var expression in expressions)
            {
                expression.Run(cityList, resultList);
            }
        }
    }
}
