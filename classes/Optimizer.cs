using System;
using System.Collections.Generic;
using System.Linq;
namespace wtest.classes
{
    public class Optimizer
    {
        private int?[] potentialsY;
        private int?[] potentialsX;
        private int?[,] potentials;
        public Rout[][] routs {get; set;}
        private int MaxValue;
        private Index MaxIndex;
        List<Vertex> loop;
        public Optimizer(Rout[][] routs){
            this.routs = routs;
        }
        private void CalculatePotentials(){
            potentialsY = new int?[routs.Length];
            potentialsX = new int?[routs[0].Length];
            potentialsY[0] = 0;

            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[i].Length; j++){
                    if(routs[i][j].goods == null){
                        continue;
                    }
                    if(potentialsX[j] == null){
                        potentialsX[j] = routs[i][j].length - potentialsY[i];
                    }
                    for(int k = 0; k < routs.Length; k++){
                        if(routs[k][j].goods == null){
                            continue;
                        }
                        if(potentialsY[k] == null){
                            potentialsY[k] = routs[k][j].length - potentialsX[j];
                        }
                    }
                }
            }
        }
        public bool IsOptimal(){
            CalculatePotentials();
            potentials = new int?[routs.Length, routs[0].Length];
            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[i].Length; j++){
                    if(routs[i][j].goods == null){
                        potentials[i,j] = (int?) potentialsX[j] + potentialsY[i] - routs[i][j].length;
                    }
                }
            }
            bool result;
            MaxValue = int.MinValue;
            MaxIndex = new Index();
            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[0].Length; j++){
                    if(potentials[i,j] > MaxValue){
                        MaxValue = (int) potentials[i,j];
                        MaxIndex.x = j;
                        MaxIndex.y = i;
                    }
                }
            }
            if(MaxValue > 0){
                result = false;
            } else{
                result = true;
            }
            Console.WriteLine(MaxValue);
            Console.WriteLine(MaxIndex.ToString());
            return result;
        }
    private List<Rout> CheckHorizontaly(int x, int y){
        List<Rout> neighbors = new List<Rout>(); 
        for(int i =0; i < routs[y].Length; i++){
            Rout selected = routs[y][i];
            if(selected.goods > 0 && selected.x != x){
                neighbors.Add(selected);
            }
        }
        return neighbors;
    }
     private List<Rout> CheckVerticaly(int x, int y){
        List<Rout> neighbors = new List<Rout>(); 
        for(int i =0; i < routs.Length; i++){
            Rout selected = routs[i][x];
            if(selected.goods > 0 && selected.y != y){
                neighbors.Add(selected);
            }
        }
        return neighbors;
     }
    private List<Rout> GetNeighbors(int x, int y){
        List<Rout> neighbors = CheckHorizontaly(x,y);
        neighbors.AddRange(CheckVerticaly(x,y));
        neighbors.OrderByDescending(n => (int) n.goods).ToList();
        //neighbors.ForEach(n => Console.WriteLine(n.ToString()));
        return neighbors;
    }
    private bool IsLooped(){
        bool loopedVerticaly = loop[loop.Count() - 1].isRow == true && loop[0].rout.x == loop[loop.Count() - 1].rout.x;
        bool loopedHorizontaly = loop[loop.Count() - 1].isRow == false && loop[0].rout.y == loop[loop.Count() - 1].rout.y;
        return loopedHorizontaly || loopedVerticaly;
    } 
    private void AddToLoop(int x, int y){
        List<Rout> neighbors = GetNeighbors(x, y);
        neighbors.OrderBy(n => n.goods);
        foreach(var neighbor in neighbors){
            List<Rout> nextNeighbors = GetNeighbors(neighbor.x,neighbor.y);
            if(nextNeighbors.Count() > 0){
                Rout nextNeighbor = nextNeighbors[0];
                if(nextNeighbor.y == neighbor.y && CheckHorizontaly(nextNeighbor.x,nextNeighbor.y).Count == 0){
                    continue;
                } else if(nextNeighbor.x == neighbor.x && CheckVerticaly(nextNeighbor.x,nextNeighbor.y).Count == 0){
                    continue;
                }
                Vertex toAdd;
                if(loop.Count() > 1){
                    toAdd = new Vertex(neighbor, !loop[loop.Count() -1].isPositive);
                    if(loop.Contains(toAdd)){
                        continue;
                    }
                    if(neighbor.x == loop[loop.Count() -1].rout.x && neighbor.y != loop[loop.Count() -1].rout.y){
                        toAdd.isRow = false;
                    } else {
                        toAdd.isRow = true;
                    }
                } else {
                    toAdd = new Vertex(neighbor, true);
                }
                if(loop.Contains(toAdd)){
                    continue;
                }
                loop.Add(toAdd);
                break;
            }
        }
    }
    public void CalculateLoop(){
        loop = new List<Vertex>();
        Vertex Head = new Vertex(routs[MaxIndex.y][MaxIndex.x],true);
        loop.Add(Head);
        List<Rout> neighbors = GetNeighbors(MaxIndex.x, MaxIndex.y);
        
        foreach(var neighbor in neighbors){
            loop = new List<Vertex>();
            loop.Add(Head);
            Vertex vertex = new Vertex(neighbor,false);
            if(MaxIndex.x == neighbor.x){
                vertex.isRow = false;
            } else {
                vertex.isRow = true;
            }
            loop.Add(vertex);
            for(int i = 1 ; i < routs.Length + routs[0].Length -1; i++){
                if(!IsLooped()){
                Vertex last = loop[loop.Count - 1];
                AddToLoop(last.rout.x,last.rout.y);       
                } else {
                    break;
                }
            if(IsLooped()){
                break;
            }
        }
        }
        Console.WriteLine("\nCycle: ");
        loop.ForEach(n => Console.WriteLine(n.ToString()));
    }

    public Rout[][] Optimize(){
        IsOptimal();
        CalculateLoop();
        int toMove = (int) loop.FindAll(n => n.isPositive).Min(n => n.rout.goods);
        Console.WriteLine("\nGoods to move: " + toMove);
        foreach(var vertex in loop){
            if(vertex.isPositive){
                if(routs[vertex.rout.y][vertex.rout.x].goods != null){
                    routs[vertex.rout.y][vertex.rout.x].goods += toMove;
                } else {
                    routs[vertex.rout.y][vertex.rout.x].goods = toMove;
                }
            } else{
                routs[vertex.rout.y][vertex.rout.x].goods -= toMove;
            }
        }  
        return routs;
    }

    }
    public struct Vertex{
        public Rout rout {get;}
        public bool isPositive {get;}
        public bool? isRow {get; set;}
        public Vertex(Rout rout, bool positive){
            this.rout = rout;
            this.isPositive = positive;
            this.isRow = null;
        }
        public override string ToString()
        {
            return $"Rout: {rout.ToString()}, positive: {isPositive}, is row: {isRow}";
        }
    }
}