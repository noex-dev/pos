using AbcRobotCore;
using Roboter.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public class UntilExpression : Expression
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
            while (!condition.IsTrue(robot))
            {
                block.Run(robot);
            }
        }
    }
}
