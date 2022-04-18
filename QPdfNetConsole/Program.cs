using System;

namespace QPdfNetConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var qPdf = new QPdfNet.Job();
            qPdf.InputFile("d:\\test.pdf")
                .Check()
                .Run(out var output);

            Console.Write("Kees: " + output);
        }
    }
}
