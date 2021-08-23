using System;
using wtest.classes;
namespace wtest
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("__________First test__________");
            int[] input = {0, 10, 0, 8, 3, 10, 7, 0, 9, 3};
            Strategy strategy = new Strategy();
            //strategy.makeEqual(input);

            //Console.WriteLine("\n__________Second test__________");
            //input = new int[] {13, 8, 28, 21, 30, 6, 13, 27, 23, 1};
            //strategy = new Strategy();
            //strategy.makeEqual(input);

            //Console.WriteLine("\n__________Third test__________");
            //input = new int[] {6, 14, 22, 12, 6, 25, 15, 14, 29, 21, 11, 14, 25, 13, 13};
            //strategy = new Strategy();
            strategy.makeEqual(input);
            
        }
    }
}