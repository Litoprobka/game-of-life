using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public partial class Board
    {
        public Board(int SizeX, int SizeY, int[] nAct)
        {
            nCount = new Dictionary<Pos?, int>();
            cells = new Dictionary<Pos?, Cell>();
            sizeX = SizeX;
            sizeY = SizeY;
            nAction = nAct;
        }
        public void NUpdate(Pos pos, int mult) //position of the new/to-be-destryed cell, multiplyer for nCount
        {
            Pos i = new Pos();
            for (i.x = pos.x-1; i.x <= pos.x+1; i.x++)
            {
                for (i.y = pos.y-1; i.y <= pos.y+1; i.y++)
                {
                    if (i != pos)
                    {
                        int val;
                        if (nCount.TryGetValue(i, out val))
                            nCount[i] += mult;
                        else nCount.Add(i, 1);
                    }
                    //Base.RedrawCell(i, nCount[i], true); 
                }
            }
        }
        public void Update()
        {
            while (crt.Count > 0) NUpdate(crt.Pop(), 1);
            while (dst.Count > 0) NUpdate(dst.Pop(), -1);
            while (re.Count > 0) Base.RedrawCell(re.Pop(), 0);
            while (ra.Count > 0) Base.RedrawCell(ra.Pop(), 1);
            while (rb.Count > 0) Base.RedrawCell(rb.Pop(), 2);
            while (rd.Count > 0) Base.RedrawCell(rd.Pop(), 3);
            while (lnk.Count > 0) LinkCell(lnk.Pop());
            while (unl.Count > 0) UnlinkCell(unl.Pop());
            while (dstc.Count > 0) DestroyCell(dstc.Pop());
        }
        public bool Iteration()
        {
            Pos? pos = link;
            while(pos != null)
            {
                if (GetCell(pos).state == 2) //if the cell had 'just born' state
                {
                    GetCell(pos).state = 1; //change its state to 'alive' 
                    ra.Push((Pos)pos); //Request a cell redraw
                }
                if (GetCell(pos).state == 0) //if the cell had 'just died' state
                {
                    switch (nAction[nCount[pos]]) 
                    {
                        case 2: //if a new cell should be born in its place
                        case 3:
                            GetCell(pos).state = 2; //change the cell state to 'just born'
                            crt.Push((Pos)pos); //request an nCount update
                            rb.Push((Pos)pos); //request a cell redraw
                            break;

                        default: //if a new cell shouldn't be born in its place
                            re.Push((Pos)pos); //request a cell redraw
                            dstc.Push((Pos)pos); //request a cell destruction
                            break;
                    }
                }
                if (GetCell(pos).state == 1) //if the cell is alive
                switch (nAction[nCount[pos]]) 
                {
                        case 0: //if the cell should die
                        case 3: 
                            DestroyCellIT((Pos)pos);
                            break;
                        
                        case 1: //if the cell shouldn't die
                        case 2:
                            unl.Push((Pos)pos); //Request a cell unlinking
                            break;
                }
                Pos i = new Pos();
                Pos lpos = (Pos)pos; //used to fix an error with nullable Pos
                for (i.x = lpos.x-1; i.x <= lpos.x+1; i.x++)
                {
                    for (i.y = lpos.y-1; i.y <= lpos.y+1; i.y++)
                    {
                        if (i != pos)
                        {
                            if (!(CellExists(i))) //if there's no cell
                            {
                                switch (nAction[nCount[i]])
                                {
                                    case 2: //if a cell should be born
                                    case 3:
                                        CreateCell(i, 2, false); //create a cell without linking it
                                        crt.Push(i); //request an nCount update
                                        rb.Push(i); //request a cell redraw
                                        lnk.Push(i); //request a cell linking
                                        break;
                                }
                            }
                            else
                            if (!GetCell(i).isLinked && GetCell(i).state == 1) //if there's an alive unlinked cell
                            {
                                switch (nAction[nCount[i]])
                                {
                                    case 0: //if the cell should die
                                    case 3:
                                        DestroyCellIT(i);
                                        lnk.Push(i); //additionally, request to link the cell
                                        break;
                                }
                            }   
                        }
                    }
                }
                pos = GetCell(pos).linksTo;
            }       
            Update(); //#3: Update the neighbour count and redraw all the changed cells.
            return link != null;
        }
        public Cell GetCell(Pos? pos) => cells[pos]; //pos == null ? cells[pos] : throw new IndexOutOfRangeException();
        public Cell GetCell(int x, int y) => cells[new Pos(x, y)];
        public bool CellExists(Pos? pos) => cells.ContainsKey(pos);
        public bool nCountExists(Pos? pos) => nCount.ContainsKey(pos);
    }
}