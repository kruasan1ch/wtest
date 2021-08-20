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
            //input = new int[] {2, 7, 4, 2, 4, 10, 5, 7, 2, 7};
            //strategy = new Strategy();
            strategy.makeEqual(input);

            //Console.WriteLine("\n__________Third test__________");
            //input = new int[] {0, 1, 1, 1, 1, 1, 1, 1, 1, 2};
            //strategy = new Strategy();
            //strategy.makeEqual(input);
            
        }
    }
}