using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Lexer
{
    class Token
    {
        private string Type { get; set; }
        private string Value { get; set; }
        public Token(string Type,string Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
        public override string ToString()
        {
            return string.Format("< {0} , {1} >", Type, Value);
        }
    }
}
