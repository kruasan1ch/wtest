using System;
using System.Collections.Generic;
namespace wtest.classes
{
    public class Searcher
    {
        private arrayItem left;
        private arrayItem right;
        private int[] source;
        private int Length;
        private int index;
        public Searcher(int[] source, int index){
            this.source = source;
            Length = source.Length;
            this.index = index;
        }
        public arrayItem searchLeft(){
            
            for ( int i = index; i >= 0; i--)
            {
                if(source[i] < 0){
                    left = new arrayItem(i, source[i]);
                    break;
                }
                if(i == 0  ){ // && source[0] <= 0
                    i = Length;
                }
            }
            return left;
        }
        public arrayItem searchRight(){
            for (int i = index; i < Length; i++)
            {
                if(source[i] < 0){
                    right = new arrayItem(i, source[i]);
                    break;
                }
                if(i == Length - 1){
                    i = -1;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                }
            }
            return right;
        }
        public paths calculatePath(){
            int leftpath = 0, rightpath = 0;
            if(index  > left.index){
                leftpath = index - left.index ;
            } else {
                leftpath = Length - left.index;
            }
            if(index  < right.index){
                rightpath = right.index - index;
            } else {
                rightpath = right.index + (Length - index);
            }
            return new paths(leftpath, rightpath);
        }
        private Dictionary<string, int> setLeft(paths paths){
            Dictionary<string, int> resultPath = new Dictionary<string, int>();
            resultPath.Add("length", paths.leftpath);
            resultPath.Add("index", left.index);
            resultPath.Add("direction", 1);
            return resultPath;
        }
        private Dictionary<string, int> setRight(paths paths){
            Dictionary<string, int> resultPath = new Dictionary<string, int>();
            resultPath.Add("length", paths.rightpath);
            resultPath.Add("index", right.index);
            resultPath.Add("direction", 0);
            return resultPath;
        }
        public Dictionary<string, int> doSearch(){
            Dictionary<string, int> resultPath = new Dictionary<string, int>();
            left = searchLeft();
            right = searchRight();
            //Console.WriteLine(left.ToString() + "\n" + right.ToString());
            paths paths = calculatePath();
            //Console.WriteLine("paths :" + paths.ToString());
            if(paths.leftpath < paths.rightpath){
                resultPath = setLeft(paths);
            } else {
                resultPath = setRight(paths);
            }
            return resultPath;
        }
    }
    public struct arrayItem{
        public int index {get;}
        public int value {get;}
        public arrayItem(int index, int value){
            this.index = index;
            this.value = value;
        }
        public override string ToString() => $"(i {index}, v {value})";
    }
    public struct paths{
        public int leftpath {get;}
        public int rightpath {get;}
        public paths(int leftpath, int rightpath){
            this.leftpath = leftpath;
            this.rightpath = rightpath;
        }
        public override string ToString() => $"L {leftpath}, R {rightpath}";
    }
}