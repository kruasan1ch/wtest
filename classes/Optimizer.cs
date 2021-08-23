using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
namespace wtest.classes
{
    public class Optimizer
    {
        private int?[] potentialsY;
        private int?[] potentialsX;
        private int?[,] potentials;
        public Rout[][] routs {get; set;}
        private int MaxValue;
        private Point MaxIndex;

        public Optimizer(Rout[][] routs){
            this.routs = routs;
            MaxValue = int.MinValue;
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
            MaxIndex = new Point();
            for(int i = 0; i < routs.Length; i++){
                for(int j = 0; j < routs[0].Length; j++){
                    if(potentials[i,j] > MaxValue){
                        MaxValue = (int) potentials[i,j];
                        MaxIndex.X = j;
                        MaxIndex.Y = i;
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
        private List<Point> CalculatePath(List<Point> Cycle, List<Point> filled){
            var arr = filled.Where(x => x.X == Cycle.Last().X && x.Y < Cycle.Last().Y);
            Point nearesTopPoint = arr.Count() == 0 ? new Point(-1, -1) : arr.First(x => Cycle.Last().Y - x.Y == arr.Min(z => Cycle.Last().Y - z.Y));

            arr = filled.Where(x => x.X == Cycle.Last().X && x.Y > Cycle.Last().Y);
            Point nearestBottomPoint = arr.Count() == 0 ? new Point(-1, -1) : arr.First(x => x.Y - Cycle.Last().Y == arr.Min(z => z.Y - Cycle.Last().Y));

            arr = filled.Where(x => x.Y == Cycle.Last().Y && x.X < Cycle.Last().X);
            Point nearestLeftPoint = arr.Count() == 0 ? new Point(-1, -1) : arr.First(x => Cycle.Last().X - x.X == arr.Min(z => Cycle.Last().X - z.X));

            arr = filled.Where(x => x.Y == Cycle.Last().Y && x.X > Cycle.Last().X);
            Point nearestRightPoint = arr.Count() == 0 ? new Point(-1, -1) : arr.First(x => x.X - Cycle.Last().X == arr.Min(z => z.X - Cycle.Last().X));

            List<Point> result = null;

            if (Cycle.Count > 3 && (nearestBottomPoint == Cycle[0] || nearestLeftPoint == Cycle[0] || nearesTopPoint == Cycle[0] || nearestRightPoint == Cycle[0]))
            {
                return Cycle;
            }
            if (nearesTopPoint != new Point(-1, -1) && !Cycle.Contains(nearesTopPoint))
                result = CalculatePath(new List<Point>(Cycle) { nearesTopPoint }, filled);
            if (result != null) return result;
            if (nearestBottomPoint != new Point(-1, -1) && !Cycle.Contains(nearestBottomPoint))
                result = CalculatePath(new List<Point>(Cycle) { nearestBottomPoint }, filled);
            if (result != null) return result;
            if (nearestLeftPoint != new Point(-1, -1) && !Cycle.Contains(nearestLeftPoint))
                result = CalculatePath(new List<Point>(Cycle) { nearestLeftPoint }, filled);
            if (result != null) return result;

            
            if (nearestRightPoint != new Point(-1, -1) && !Cycle.Contains(nearestRightPoint))
                result = CalculatePath(new List<Point>(Cycle) { nearestRightPoint }, filled);
            if (result != null) return result;

            return null;
        }
        public List<Point> TrimPath(List<Point> Path){
           bool NotOptimalt = true;
            while (NotOptimalt)
            {
                int countOfPointsInLine = 1;
                bool horizontalMovement = true;
                for (int i = 1; i < Path.Count; i++)
                {
                    if (horizontalMovement && Path[i].X != Path[i-1].X)
                    {
                        horizontalMovement = false;
                        countOfPointsInLine = 1;
                    }
                    else if (!horizontalMovement && Path[i].Y != Path[i-1].Y)
                    {
                        horizontalMovement = true;
                        countOfPointsInLine = 1;
                    }
                    countOfPointsInLine++;
                    if (countOfPointsInLine>2)
                    {
                        Path.RemoveAt(i - 1);
                        break;
                    }
                    if (i==Path.Count-1)
                    {
                        NotOptimalt=false;
                    }
                }
                while (Path[0].X == Path[Path.Count - 2].X || Path[0].Y == Path[Path.Count-2].Y){
                    Path.RemoveAt(Path.Count - 1);
                }
            }
            return Path;
        }        
        public Rout[][] Optimize(){ 
                IsOptimal();
                List<Point> filled = TableConstructor.findFilled(routs);
                routs = TableConstructor.fixDegeneracy(routs, filled.Count());
                filled.Add(MaxIndex);
                List<Point> path = TrimPath(CalculatePath(new List<Point>{MaxIndex}, filled));
                bool positive = true;
                List<Vertex> loop = new List<Vertex>();
                foreach(var step in path){
                    loop.Add(new Vertex(routs[step.Y][step.X], positive));
                    positive = !positive;
                }
                int toMove = (int) loop.Min(n => n.rout.goods);
                foreach(var vertex in loop){
                    if(vertex.isPositive){
                        if(routs[vertex.rout.y][vertex.rout.x].goods != null){
                            routs[vertex.rout.y][vertex.rout.x].goods += toMove;
                        } else {
                            routs[vertex.rout.y][vertex.rout.x].goods = toMove;
                        }
                    } else {
                        routs[vertex.rout.y][vertex.rout.x].goods -= toMove;
                        if(routs[vertex.rout.y][vertex.rout.x].goods == 0){
                            routs[vertex.rout.y][vertex.rout.x].goods = null;
                        }
                    }
                }
            
            return routs;
        }
        public struct Vertex{
            public Rout rout {get;}
            public bool isPositive {get;}
            public Vertex(Rout rout, bool positive){
                this.rout = rout;
                this.isPositive = positive;
            }
            public override string ToString()
            {
                return $"Rout: {rout.ToString()}, positive: {isPositive}";
            }
        }
    }
}