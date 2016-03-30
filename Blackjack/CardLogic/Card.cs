using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLogic
{
    public class Card 
    {
        public string Suite { get; }
        public int Value { get; }
        public bool IsHidden { get; set; }
        public Card(string suite, int value)
        {
            Suite = suite;
            Value = value;
            IsHidden = false;
        }
        public void ToggleHidden()
        {
            IsHidden = !IsHidden;
        }
        public override string ToString()
        {
            if (IsHidden)
                return "[]";
            else
            {
                if (Value == 1)
                    return "A" + Suite;
                if (Value == 11)
                    return "J" + Suite;
                if (Value == 12)
                    return "Q" + Suite;
                if (Value == 13)
                    return "K" + Suite;
                else
                    return Value + Suite;
            }
        }
    }
}
