using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter.Tokenizer
{
    public class Token
    {
        public enum TokenType { Keyword, Direction, Target, Number, Letter, OpenBracket, CloseBracket, Error }
        public required string Value { get; set; }
        public required TokenType Type { get; set; }
    }
}
