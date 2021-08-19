namespace wtest.classes
{
    public class LogisticItem
    {
        public int index{get;}
        public int value{get; set;}

        public LogisticItem(int index, int value){
            this.index = index;
            this.value = value;
        }
        public override string ToString()
        {
            return $"\nIndex: {index}, Value: {value}";
        }
    }
}