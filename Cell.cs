using System;

namespace GameOfLife 
{
    public class Cell
    {
        public Cell(int State)
        {
            linksTo = null;
            state = State;
            isLinked = false;
        }     
        public Pos? linksTo;
        public Pos? linkedFrom;
        public bool isLinked;
        public int state; //0 = just died; 1 = alive; 2 = just born;
    }
}