using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGry
{
    public class Attempt
    {
        public readonly int Number;
        public readonly Answer Answer;

        public Attempt(int number, Answer answer)
        {
            Number = number;
            Answer = answer;
        }
    }
}
