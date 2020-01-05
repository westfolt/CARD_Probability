using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    static class FSPECOperations
    {
        public static bool[] AddFSPEC(this bool[] InitialFSPEC, bool[] AddedFSPEC)
        {
            bool[] output = ResizeFSPEC(InitialFSPEC);
            for (int i = output.Length - 8, j = 0; j < AddedFSPEC.Length; i++, j++)
            {
                output[i] = AddedFSPEC[j];
            }

            return output;
        }
        private static bool[] ResizeFSPEC(bool[] input)
        {
            bool[] output = new bool[input.Length + 8];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }

            return output;
        }
    }
}
