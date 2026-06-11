using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public class Condition
    {
        private Tokenizer.Token? direction;
        private Tokenizer.Token? target_or_letter;

        public void Parse(List<Tokenizer.Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.Direction)
            {
                direction = tokens[0];
                tokens.RemoveAt(0);
            }

            else
            {
                Expression.Errors.Add("Expected direction");
                return;
            }

            if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.Keyword && tokens[0].Value == "IS-A")
            {
                tokens.RemoveAt(0);
            }

            else
            {
                Expression.Errors.Add("Expected 'IS-A'");
                return;
            }

            if (tokens.Count > 0 && (tokens[0].Type == Tokenizer.Token.TokenType.Target
                || tokens[0].Type == Tokenizer.Token.TokenType.Letter))
            {
                target_or_letter = tokens[0];
                tokens.RemoveAt(0);
            }

            else
            {
                Expression.Errors.Add("Expected target or letter");
            }
        }

        public bool IsTrue(RobotField robot)
        {
            if (robot == null || direction == null ||  target_or_letter == null)
            {
                return false;
            }

            RobotField.Direction dir = direction.Value switch
            {
                "LEFT" => RobotField.Direction.Left,
                "RIGHT" => RobotField.Direction.Right,
                "UP" => RobotField.Direction.Up,
                "DOWN" => RobotField.Direction.Down,
                _ => throw new InvalidOperationException($"Unknown direction: {direction.Value}")
            };

            if (target_or_letter.Type == Tokenizer.Token.TokenType.Target)
            {
                return robot.IsObstacle(dir);
            }

            else
            {
                return robot.IsLetter(target_or_letter.Value, dir);
            }
        }
    }
}
