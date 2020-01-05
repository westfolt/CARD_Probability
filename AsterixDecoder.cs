using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    abstract class AsterixDecoder
    {
        public abstract bool[] FSPEC { get; set; }
        public abstract Asterix[] Decode(string[] bytes, int pointer);
    }
}