using System.Drawing;
namespace wtest.classes
{
    public class Rout
    {
        public int? goods {get; set;}
        public int length {get;}
        public int x {get;} 
        public int y {get;}
        public Rout(int x, int y, int length){
            this.x = x;
            this.y = y;
            this.length = length;
            this.goods = -1;
        }
        public Rout(){
            
        }
        public override string ToString(){
            return $"Goods: {goods}, length: {length}, X: {x}, Y: {y}";
        }
        public Point ToPoint(){
            return new Point(x,y);
        }
    }
}