using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLighting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Window win = new Window(600, 600, "light"))
            {
                win.Run(60);
            }
        }
    }
}
