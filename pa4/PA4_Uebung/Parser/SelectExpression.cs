using DataModel;
using PA4_Uebung.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Uebung.Parser
{
    public class SelectExpression : Expression
    {
        public override void Parse(List<Token> tokens) { }

        public override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            resultList.AddRange(cityList); // alles zurückgeben
        }
    }
}
