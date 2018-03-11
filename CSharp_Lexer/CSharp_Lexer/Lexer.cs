using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSharp_Lexer
{
    class Lexer
    {
        private string TextInput { get; set; }
        private int Position { get; set; } = 0;
        private char CurrentChar { get; set; }

        List<char> WhiteSpaces = new List<char> { ' ','\n','\t' };
        List<char> Numbers =new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<Token> Tokens = new List<Token>();
        List<char> Chars = new List<char> { '_','a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<char> Alphas = new List<char> { '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        List<string> ReservedKeywords = new List<string> { "Var","int","float", "char", "class","interface","abstract","override","if","else","do","while","break","continue","public","private","void"};
        List<char> Operators = new List<char> { '+', '-', '*', '/', '>', '<','&','|' };
        List<char> Symbols = new List<char> { '=', '{', '}', '(', ')', ':', '.', ',', ';' };

        public Lexer(string TextInput)
        {
            this.TextInput = TextInput;
            CurrentChar = TextInput[Position];
        }

        void Getch()
        {
            Position += 1;
            if (Position > TextInput.Length - 1)
            {
                CurrentChar = '$';
                Tokens.Add(new Token("EOF", "$"));
            }
            else
            {
                Debug.WriteLine(CurrentChar);
                Debug.WriteLine((int)CurrentChar);
                CurrentChar = TextInput[Position];
            }
        }
        void Ungetch()
        {
            Position -= 1;
        }

        char Peek()
        {
            int PeekPosition = Position + 1;
            if (Position > TextInput.Length - 1)
            {
                return '$';
            }
            else
            {
                return TextInput[PeekPosition];
            }
        }

        void Skip_Whitespace()
        {
            while(CurrentChar!='$' & WhiteSpaces.Contains(CurrentChar))
            {
                Getch();
            }
        }
        
        void Skip_Comment()
        {
            while(CurrentChar != '$' & CurrentChar!='*')
            {
                Getch();
            }
            if(CurrentChar == '*')
            {
                if (Peek() == '/')
                {
                    Getch();
                    Getch();
                }
                else
                {
                    Getch();
                    Skip_Comment();
                }
            }
            
        }
        void Number()
        {
            string Result = "";
            while (CurrentChar != '$' & Numbers.Contains(CurrentChar))
            {
                Result += CurrentChar.ToString();
                Getch();
            }
            if (CurrentChar == '.')
            {
                Numbers.Add('e');
                Result += CurrentChar.ToString();
                Getch();
                while (CurrentChar != '$' & Numbers.Contains(CurrentChar))
                {
                    Result += CurrentChar.ToString();
                    Getch();
                }
                Tokens.Add(new Token("float", Result));
                Numbers.Remove('e');
            }
            else
            {
                Tokens.Add(new Token("int", Result));
            }
        }
        void Identifier()
        {
            string Result = "";
            while (CurrentChar != '$' & Chars.Contains(CurrentChar))
            {
                Result += CurrentChar;
                Getch();
            }
            if (ReservedKeywords.Contains(Result))
            {
                Tokens.Add(new Token("Keyword", Result));
            }
            else
            {
                Tokens.Add(new Token("Identifier", Result));
            }
        }

        void Handle_Operator()
        {
            string Result = CurrentChar.ToString();
            if (Peek() == '=')
            {
                Getch();
                Result += CurrentChar.ToString();
                Getch();
            }
            Tokens.Add(new Token("Operator", Result));
            Getch();
        }
        
        void Handle_OtherSymbols()
        {
            if (CurrentChar == '=')
            {
                Tokens.Add(new Token("Assignment", "="));
            }
            if (CurrentChar == '{')
            {
                Tokens.Add(new Token("Open braces", "{"));
            }
            if (CurrentChar == '}')
            {
                Tokens.Add(new Token("Close braces", "}"));
            }
            if (CurrentChar == '(')
            {
                Tokens.Add(new Token("Open Paranthesis", "("));
            }
            if (CurrentChar == ')')
            {
                Tokens.Add(new Token("Close Paranthesis", ")"));
            }
            if (CurrentChar == ':')
            {
                Tokens.Add(new Token("Colon", ":"));
            }
            if (CurrentChar == '.')
            {
                Tokens.Add(new Token("Dot", "."));
            }
            if (CurrentChar == ',')
            {
                Tokens.Add(new Token("Comma", "."));
            }
            if (CurrentChar == ';')
            {
                Tokens.Add(new Token("SemiColon", ";"));
            }
            Getch();
        }

        //Errors
        void Error_Handler()
        {
            Tokens.Add(new Token("Error", CurrentChar.ToString()));
            Getch();
        }
        string x = "" +
            "";
        public List<Token> Tokenize()
        {
            while (CurrentChar !='$'){
                bool Error = true;
                //Whitespaces
                if (WhiteSpaces.Contains(CurrentChar) || CurrentChar.ToString()=="\n" || x.Contains(CurrentChar.ToString()))
                {
                    Skip_Whitespace();
                    Error = false;
                }

                //Comments
                if (CurrentChar == '/')
                {
                    if (Peek() == '*')
                    {
                        Getch();
                        Getch();
                        Skip_Comment();
                        Error = false;
                    }
                }

                // Handle \n
                if((int)CurrentChar == 13 || (int)CurrentChar == 10)
                {
                    Getch();
                    Error = false;
                }
                //Identifier or Keyword
                if (Alphas.Contains(CurrentChar))
                {
                    Identifier();
                    Error = false;
                }

                //Number
                if (Numbers.Contains(CurrentChar))
                {
                    Number();
                    Error = false;
                }

                //Operators
                if (Operators.Contains(CurrentChar))
                {
                    Handle_Operator();
                    Error = false;
                }

                if (Symbols.Contains(CurrentChar))
                {
                    Handle_OtherSymbols();
                    Error = false;
                }
                if (Error)
                {
                    Error_Handler();
                }
                
            }
            return Tokens;
        }

        
    }
}
