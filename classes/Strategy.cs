using System.Linq;
using System.Collections.Generic;
using System;
using System.Drawing;
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
            List<Point> Points = new List<Point>();
            foreach(var point in filled){
                Points.Add(point.ToPoint());
            }
            Optimizer optimizer = new Optimizer(routs, Points);
            while(optimizer.IsOptimal() == false){
                //routs = optimizer.Optimize();
                optimizer.Optimize();
                Console.WriteLine($"Result: {CountResult()}");
                break;
            }
            Console.WriteLine("Plan is optimal");
            
            Console.WriteLine($"Result: {CountResult()}");
        }
    }
}