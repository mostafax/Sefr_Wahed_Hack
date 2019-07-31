using System;
using System.Collections.Generic;
using System.Text;

namespace wasfetyMobile
{
    class Pair<TFirst, TSecond>
    {
        public TFirst First;
        public TSecond Second;
        public Pair() {}
        public Pair(TFirst First, TSecond Second) { this.First = First; this.Second = Second; }
    }
}
