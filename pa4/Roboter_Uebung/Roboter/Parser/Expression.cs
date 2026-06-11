using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Parser
{
    public abstract class Expression
    {
        public static List<string> Errors { get; set; } = [];
        public abstract void Parse(List<Tokenizer.Token> tokens);
        public virtual void Run(RobotField robot) { }
    }
}
