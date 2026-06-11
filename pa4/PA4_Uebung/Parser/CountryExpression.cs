using DataModel;
using PA4_Uebung.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Uebung.Parser
{
    public class CountryExpression : Expression
    {
        private Token country;
        private Expression block = new BlockExpression();

        public override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.Country)
            {
                country = tokens[0];
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("COUNTRY: Ländername erwartet");
                return;
            }
            block.Parse(tokens);
        }

        public override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            var countryCities = cityList.Where(c => c.Country == country.Value).ToList();
            block.Run(countryCities, resultList);
        }
    }
}
