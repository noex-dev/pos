using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public class CollectExpression : Expression
    {

        public override void Parse(List<Tokenizer.Token> tokens)
        {
            // Nothing after 'COLLECT' --> Skip Parsing
        }

        public override void Run(RobotField robot)
        {
            if (string.IsNullOrEmpty(robot.Collect()))
            {
                Errors.Add("Nothing to collect");
            }
        }
    }
}
