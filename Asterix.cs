using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    abstract class Asterix
    {
        public byte Category { get; }

        public Asterix(byte Category)
        {
            this.Category = Category;
        }
    }
}