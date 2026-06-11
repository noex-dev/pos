using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Uebung.Tokenizer
{
    public class Token
    {
        public enum TokenType { Keyword, Country, OpenBracket, CloseBracket, Error }
        public required string Value { get; set; }
        public required TokenType Type { get; set; }
    }
}