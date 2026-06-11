using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public class MoveExpression : Expression
    {
        private Tokenizer.Token? direction;

        public override void Parse(List<Tokenizer.Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.Direction)
            {
                direction = tokens[0];
                tokens.RemoveAt(0);
            }

            else
            {
                Errors.Add("Expected direction");
            }
        }

        public override void Run(RobotField robot)
        {
            switch(direction?.Value)
            {
                case "LEFT":
                    robot.Move(RobotField.Direction.Left); break;

                case "RIGHT":
                    robot.Move(RobotField.Direction.Right); break;

                case "UP":
                    robot.Move(RobotField.Direction.Up); break;

                case "DOWN":
                    robot.Move(RobotField.Direction.Down); break;
            }
        }
    }
}
