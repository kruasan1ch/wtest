using System.Linq;
using System.Collections.Generic;
using System;
namespace wtest.classes
{
    public class Strategy
    {
       private int[] data;
       private int[] profits;
       private int Length;
       public Strategy(int[] data){
           this.data = data;
           Length = data.Length;
       }
        private void calculateProfits(){
            int average = (int) data.Average();
            int[] profits = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                profits[i] = data[i] - average;
            }
            this.profits = profits;
        }
        private int calculateMoves(int index){
            int result = 0;
            for (int i = 0; i < Length; i++)
            {
                
            }
            return result;
        }
        private int calculateMoves(){
            
            Searcher searcher;
            int moves = 0;
            for (int i = 0; i < Length; i++)
            {
                if(profits[i] <= 0){
                    continue;
                }
                searcher = new Searcher(profits, i);
                Dictionary<string, int> move = searcher.doSearch();
                string direction = move["direction"] == 0 ? "right" : "left";
                int delta = profits[i] + profits[move["index"]];
                int points = 0;
                if(delta <= 0){
                    points = profits[i];
                    profits[i] = 0;
                    profits[move["index"]] += points;
                } else {
                    points =  Math.Abs(profits[move["index"]]);
                    profits[i] -= points;
                    profits[move["index"]] = 0;
                }

                moves += Math.Abs(points) * move["length"];
                
                Console.WriteLine($"index from: {i}, index to: {move["index"]} direction: {direction}, delta: {delta}, points: {points}, length: {move["length"]}");
            }
            //Console.WriteLine($"\nmoves: {moves}\n");
            //profits.ToList().ForEach(n => Console.WriteLine($"value: {n}"));
            return moves;
        }
        public void makeEqual(){
            calculateProfits();
            Console.WriteLine("__________Initial profits_________");
            profits.ToList().ForEach(n => Console.WriteLine($"value: {n}"));
            Console.WriteLine("_________________________________");
            int moves = calculateMoves();
            do
            {
                moves += calculateMoves();
                
            } 
            while(profits.Any(n => n > 0));
            Console.WriteLine("\ntotal moves: " + moves);
        }
        
    }
}