using System.Linq;
using System.Collections.Generic;
using System;
namespace wtest.classes
{
    public class Strategy
    {

        private Rout[][] routs;
        private List<LogisticItem> negatives;
        private List<LogisticItem> positives;
        private int invertible;
        private List<Rout> filled;
        private void printRout(){
            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[i].Length; j++){
                    Console.WriteLine(routs[i][j]);
                }
                Console.WriteLine("\r\n");
            }
        }
        private int CountResult(){
            int result = 0;
            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[i].Length; j++){
                    if(routs[i][j].goods > 0){
                        result += (int)routs[i][j].goods * routs[i][j].length;
                    } 
                }
            }
            return result;
        }
        public void makeEqual(int[] data){
            TableConstructer table = new TableConstructer(data);
            table.CalculateInitialPlan();
            routs = table.routs;
            printRout();
            Console.WriteLine($"Result: {CountResult()}");

            negatives = table.negatives;
            positives = table.positives;
            int?[] profitX = new int?[negatives.Count()];
            int?[] profitY = new int?[positives.Count()];
            profitY[0] = 0;
            
            
            Console.WriteLine("\nx");
            profitX.ToList().ForEach(i => Console.WriteLine(i));
            Console.WriteLine("\ny");
            profitY.ToList().ForEach(i => Console.WriteLine(i));

        }
        private static bool findColumn(Rout rout, int x, int y){
            return rout.y == y && rout.x != x;
        }
    }
}