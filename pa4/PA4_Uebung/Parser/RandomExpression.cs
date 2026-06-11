using DataModel;
using PA4_Uebung.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Uebung.Parser
{
    public class RandomExpression : Expression
    {
        public override void Parse(List<Token> tokens) { }

        public override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            if (cityList.Count > 0)
            {
                int index = Random.Shared.Next(cityList.Count);
                resultList.Add(cityList[index]);
            }
        }
    }
}
