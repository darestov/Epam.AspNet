using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Helpers
{
    public static class NorthwindBitmapHelper
    {
        public static byte[] GetBitmapFix(byte[] raw)
        {
            if (raw[0] == 'B' && raw[1] == 'M')
                return raw;
            else return raw.Skip(78).ToArray();
        }
    }
}
