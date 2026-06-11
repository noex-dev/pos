using DataModel;
using PA4_Uebung.Tokenizer;

namespace PA4_Uebung.Parser
{
    public abstract class Expression
    {
        public static List<string> Errors { get; set; } = [];
        public abstract void Parse(List<Token> tokens);
        public virtual void Run(List<Worldcity> cityList, List<Worldcity> resultList) { }
    }
}