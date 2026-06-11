using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public class BlockExpression : Expression
    {
        private readonly Program program = new();

        public override void Parse(List<Tokenizer.Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.OpenBracket)
            {
                tokens.RemoveAt(0);
                program.Parse(tokens);

                if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.CloseBracket)
                {
                    tokens.RemoveAt(0);
                }

                else
                {
                    Errors.Add("Expected '}'");
                }
            }

            else
            {
                Errors.Add("Expected '{'");
            }
        }

        public override void Run(RobotField robot)
        {
            program.Run(robot);
        }
    }    
}
