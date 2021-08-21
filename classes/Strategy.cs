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
            TableConstructor table = new TableConstructor(data);
            table.CalculateInitialPlan();
            routs = table.routs;
            negatives = table.negatives;
            positives = table.positives;
            printRout();
            filled = table.filled;
            Optimizer optimizer = new Optimizer(routs);
            while(optimizer.IsOptimal() == false){
                routs = optimizer.Optimize();
                optimizer.routs = routs;
                Console.WriteLine($"Result: {CountResult()}");
                printRout();
            }
            Console.WriteLine("Plan is optimal");
            
            Console.WriteLine($"Result: {CountResult()}");
        }
    }
}