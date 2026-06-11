using AbcRobotCore;
using Roboter.Tokenizer;

namespace Roboter.Parser
{
    public class IfExpression : Expression
    {
        private Condition condition = new();
        private Expression block = new BlockExpression();

        public override void Parse(List<Token> tokens)
        {
            condition.Parse(tokens);
            block.Parse(tokens);
        }

        public override void Run(RobotField robot)
        {
            if (condition.IsTrue(robot))
            {
                block.Run(robot);
            }
        }
    }
}
