namespace wtest.classes
{    public struct Index
    {
        public int x {get; set;}
        public int y {get; set;}
        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }
    }
}