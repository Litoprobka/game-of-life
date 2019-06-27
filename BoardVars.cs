using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public partial class Board
    {
        public int sizeX, sizeY;
        public int[] nAction; //0 = die; 1 = survive; 2 = create new cell / survive; 3 = create new cell / die.
        public Dictionary<Pos?, int> nCount; // A dictionary counting neighbours for all cells. Changes each time when a cell is created/destroyed.
        public Stack<Pos> crt = new Stack<Pos>(), dst = new Stack<Pos>(), 
                          re = new Stack<Pos>(), rd = new Stack<Pos>(), ra = new Stack<Pos>(), rb = new Stack<Pos>(),
                          unl = new Stack<Pos>(), lnk = new Stack<Pos>(), //crt (create) and dst (destroy) for nCount update requests, other four are for visual updates
                          dstc = new Stack<Pos>(); //A stack for cell destuction
                          
                                                  //(re = empty cell; rd = cell that just died; ra = a living cell; rb = cell that's just been born).
                                                  //unl and lnk are for linking/unlinking. 
        public Dictionary<Pos?, Cell> cells; // A dictionary containing all the cells which are alive / just born / just died.
        public Pos? link = null, lastlink = null; //The first link; a link to the last created cell.
    }
}