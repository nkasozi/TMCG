using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using TcmpTestCore;

namespace TcmpWebForm
{
    public class SharedLogic
    {
        public static TcmpCore TcmpTestCore = new TcmpCore();
 

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        
    }
}