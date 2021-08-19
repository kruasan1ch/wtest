namespace wtest.classes
{
    public class Rout
    {
        public int? goods {get; set;}
        public int length {get;}
        public int x {get;} 
        public int y {get;}
        public int potential {get; set;}
        public Rout(int x, int y, int length){
            this.x = x;
            this.y = y;
            this.length = length;
            this.goods = -1;
            this.potential = 0;
        }
        public override string ToString(){
            return $"Goods: {goods}, length: {length}, X: {x}, Y: {y}";
        }
    }
}