using System;
using wtest.classes;
namespace wtest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                Суть алгоритма заключается в поиске точек избытка и точек и точек убытка и уравнении ближайших "гор" и "ям".
                Класс Strategy занимается перераспределением фишек, класс Searcher ищет ближайшие точки убытка слева и справа относительно данной точки избытка,
                находит из них ближайшую и возвращает индекс найденной точки, длинну между точками и направление поиска.
                Некоторые методы класса Searcher можно сделать приватными, но они пока являются публичными для удобства дебага.
            */
            Console.WriteLine("__________First test__________");
            int[] input = {1, 5, 9, 10, 5};
            Strategy strategy = new Strategy(input);
            strategy.makeEqual();

            Console.WriteLine("\n__________Second test__________");
            input = new int[] {6, 2, 4, 10, 3};
            strategy = new Strategy(input);
            strategy.makeEqual();

            Console.WriteLine("\n__________Third test__________");
            input = new int[] {0, 1, 1, 1, 1, 1, 1, 1, 1, 2};
            strategy = new Strategy(input);
            strategy.makeEqual();
            
        }
    }
}