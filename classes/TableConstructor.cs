using System.Collections.Generic;
using System.Linq;
using System;
namespace wtest.classes
{
    public class TableConstructor
    {   
        private int[] data;
        private int Length;
        public Rout[][] routs {get; set;}
        public List<LogisticItem> negatives {get; set;}
        public List<LogisticItem> positives {get; set;}
        private int invertible;
        public List<Rout> filled {get; set;}
        public TableConstructor(int[] data){
            this.data = data;
            Length = data.Length;
            negatives = new List<LogisticItem>();
            positives = new List<LogisticItem>();
            filled = new List<Rout>();
            invertible = 0;
        }
        private int[] CalculateSubs(){
            int average = (int) data.Average();
            int[] SubData = new int[Length];
            for(int i = 0; i < Length; i++){
                SubData[i] = data[i] - average;
            }
            return SubData;
        }
        private void CalculateSimplex(int[] subData){
            for (int i = 0 ; i < Length; i++){
                if(subData[i] >=0){
                    positives.Add(new LogisticItem(i,subData[i]));
                } else {
                    negatives.Add(new LogisticItem(i,Math.Abs(subData[i])));
                }
            }
            positives = positives.OrderByDescending(i => i.value).ToList();
            negatives = negatives.OrderByDescending(i => i.value).ToList();
        }
        public int findClosestPath(int from, int to, int Length){
            int left = 0;
            int right = 0;

            if (from > to){
                if(from != Length -1){
                    left = Length - from + to;
                    right = from - to;
                } else {
                    left = from - to;
                    right =  to + 1;
                }   
            } else {
                left = to - from;
                right = Length - to + from;
            }

            return Math.Min(left, right);
        }
        private void CalculateRouts(){
            CalculateSimplex(CalculateSubs());
            routs = new Rout[positives.Count][];
            for(int i = 0; i < positives.Count; i++){
                routs[i] = new Rout[negatives.Count];
                for(int j = 0; j < negatives.Count; j++){
                    routs[i][j] = new Rout(j,i,findClosestPath(negatives.ElementAt(j).index, positives.ElementAt(i).index, Length));
                }
            }  
        }
        private Rout[] MinPrice(int index, Rout[] supplyLine){
            Rout[] result = supplyLine;
            int minIndex = 0;
            int minimum = int.MaxValue;
            for(int i = 0; i < result.Length; i++){
                if(result[i].length < minimum && result[i].goods < 0){
                    minimum = result[i].length;
                    minIndex = i;
                }
            }
            if(positives[index].value > 0){
                if(positives[index].value > negatives[minIndex].value){
                    result[minIndex].goods = negatives[minIndex].value;
                    positives[index].value -= negatives[minIndex].value;
                    negatives[minIndex].value = 0; 
                } else if(positives[index].value < negatives[minIndex].value){
                    result[minIndex].goods = positives[index].value;
                    negatives[minIndex].value -= positives[index].value;
                    positives[index].value = 0;
                } else if(positives[index].value == negatives[minIndex].value){
                    result[minIndex].goods = negatives[minIndex].value;
                    negatives[minIndex].value = 0;
                    positives[index].value = 0;
                }
            } else {
                result[minIndex].goods = null;
            }
            invertible++;
            if(result[minIndex].goods != null){
                filled.Add(result[minIndex]);
            }
            if(positives[index].value == 0){
                foreach (var rout in result){
                    if (rout.goods < 0){
                        rout.goods = null;
                    }
                }
            }
            return result;
        }
        private void RecalculateRout(){
            for(int i = 0; i < routs.Length; i++){
                if(routs[i].Any(i => i.goods < 0)){
                    routs[i] = MinPrice(i, routs[i]);
                }
            }
        }
        public void CalculateInitialPlan(){
            CalculateRouts();
            RecalculateRout(); 
            for(int i = 0; i < negatives.Count; i++){
                if(negatives[i].value == 0){
                    for(int j = 0; j < positives.Count; j++){
                        if(routs[j][i].goods < 0){
                            routs[j][i].goods = null;
                        }
                    }
                }
            }
            do{
                RecalculateRout();
            }
            while(negatives.Any(i => i.value > 0));
            
            int inv = negatives.Count() + positives.Count() - 1;
            if(inv > invertible){
                foreach (var rout in filled)
                {
                    if(rout.x +1 < routs[rout.y].Length && routs[rout.y][rout.x +1].goods == null){
                        routs[rout.y][rout.x + 1].goods = 0;
                        invertible++;
                        if(invertible == inv)
                            break;
                    }
                }
            }
        } 
    }
}