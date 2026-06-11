using AbcRobotCore;

namespace Roboter.Parser
{
    public class RepeatExpression : Expression
    {
        private int count = 0;
        private readonly BlockExpression block = new();

        public override void Parse(List<Tokenizer.Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Tokenizer.Token.TokenType.Number)
            {
                count = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
                block.Parse(tokens);
            }

            else
            {
                Errors.Add("Expected Number");
            }
        }

        public override void Run(RobotField robot)
        {
            for (int i = 0; i < count; i++)
            {
                block.Run(robot);
            }
        }
    }
}
