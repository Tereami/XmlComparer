using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlComparer;

namespace XmlComparerDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            string fam1 = @"C:\xml\fam1.xml";
            string fam2 = @"C:\xml\fam2.xml";

            string xml1 = System.IO.File.ReadAllText(fam1);
            string xml2 = System.IO.File.ReadAllText(fam2);

            string result = XmlComparer.Comparer.CompareXmls(xml1, xml2);

            Console.Write(result);

            Console.ReadKey();
        }
    }
}
