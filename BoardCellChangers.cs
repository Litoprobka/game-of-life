using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class Board
    {
        public void CreateCell(Pos pos, int state, bool shouldLink = true) //Creates a new cell with given coordinates and state
        {
            cells.Add(pos, new Cell(state));//used to be cells[pos.x, pos.y] = new Cell(state);
            if (shouldLink) LinkCell(pos);
            if (!nCountExists(pos)) nCount.Add(pos, 0);
            
            Debug.WriteLine($"Cell {pos} created");
            
        }
        public void LinkCell(Pos pos)
        {
            GetCell(pos).linkedFrom = lastlink; //creating a new link from the last cell to the current cell
            if (link != null) GetCell(lastlink).linksTo = pos;
                else  link = pos; //If it's the first created cell, create the required link
            lastlink = pos;
            GetCell(pos).isLinked = true;
            
            Debug.WriteLine($"Linked cell {pos}. linked from = {GetCell(pos).linkedFrom}");
        }
        public void UnlinkCell(Pos pos) //Removes the link to and from the cell
        {
            /*  1 2 3 ->   1 - 3
            L 0 1 2 - -> 0 2 - - */
            Cell c = GetCell(pos);
            if (c.linkedFrom != null) GetCell(c.linkedFrom).linksTo = c.linksTo; //if cell C is linked from a cell C2, link C2 to the cell C was linked to  
                else link = c.linksTo; //else change the first link to the cell C was linked to
            if (c.linksTo != null) GetCell(c.linksTo).linkedFrom = c.linkedFrom; //if cell C links to a cell C3, link C3 back to C2 
                else lastlink = c.linkedFrom; //else change lastlink to C2
            
            c.linksTo = null; //reset the link; linkedFrom is reset automatically when linking the cell; not resetting the link used to create an infinite loop.
            c.isLinked = false; //Cell is now marked as 'unlinked'
            
            Debug.WriteLine($"Unlinked cell {pos}. Linked from = {GetCell(pos).linkedFrom}; Linked to = {GetCell(pos).linksTo}");
        }
        public Pos? DestroyCell(Pos pos)
        {
            UnlinkCell(pos);
            Pos? r = GetCell(pos).linksTo;
            cells.Remove(pos);
            
            Debug.WriteLine($"Cell {pos} destroyed");
            
            return r;
        }
        public void CreateCellPM(Pos pos) //cell creation used by paint mode
        {
            CreateCell(pos, 1);
            NUpdate(pos, 1);
        }
        public void DestroyCellPM(Pos pos) //cell destruction used by paint mode
        {
            DestroyCell(pos);
            NUpdate(pos, -1);
        }
        public void DestroyCellIT(Pos pos)
        {
            GetCell(pos).state = 0; //Set cell state to 'just died'
            rd.Push(pos); //Request a cell redraw 
            dst.Push(pos); //Request an nCount update
        }
    }
}