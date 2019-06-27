using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace GameOfLife
{
    static partial class Base
    {
        static Dictionary<string, Board> boards;
        static int dimX, dimY;
        static string boardIndex;

        static void Main(string[] args)
        {
            boardIndex = "baseBoard";
            boards = new Dictionary<string, Board>();            
            Setup(boardIndex);

            string action;
            bool shouldContinue = true;
            while (shouldContinue)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Enter a command: ");
                action = Console.ReadLine();
                Console.SetCursorPosition(0, 0);
                ClearSpace($"Enter a command: {action}".Length);
                switch (action)
                {
        			case "run":
                        Run(boards[boardIndex]);
                        break;
                    case "paint":
                        PaintMode(boards[boardIndex]);
                        break;
                    case "exit":
                        shouldContinue = false;
                        break;
                    case "rule":
                        OverwriteRules(rulesFilePath);
                        boards[boardIndex].nAction = ReadRules(rulesFilePath);
                        break;
                    case "clear":
                        boards[boardIndex] = new Board(dimX, dimY, ReadRules(rulesFilePath));
                        Console.Clear();
                        break;
                    case "new":
                        Console.SetCursorPosition(0, 0);
                        boardIndex = Console.ReadLine();
                        Console.SetCursorPosition(0, 0);
                        ClearSpace(boardIndex.Length);
                        boards[boardIndex] = new Board(dimX, dimY, ReadRules(rulesFilePath));
                        break;
                    default:
                        Console.SetCursorPosition(0, 0); 
                        Console.WriteLine("Unknown command");
                        Thread.Sleep(500);
                        break;
                }
            }
        }
        static void ClearSpace(int length)
        {
            string spaces = "";
            for (int i = 0; i < length; i++) spaces += ' ';
            Console.Write(spaces); 
        }
        static void Setup(string bIndex)
        {
            Console.Clear();
            
            Console.CursorVisible = false;
            dimX = Console.LargestWindowWidth;
            dimY = Console.LargestWindowHeight;

            int[] acts = ReadRules(rulesFilePath);
            boards[boardIndex] = new Board(dimX, dimY, acts);
            
            Console.SetWindowSize(dimX, dimY);
            Console.Title = "Game of Life";
            Console.Clear();
        }
        static int MoveMinus(int inp) => inp > 0 ? inp-1 : inp;
        static int MovePlus(int inp, int limit) => inp < limit - 1 ? inp+1 : inp;
        public static void RedrawCell(Pos pos, int state, bool alt = false)
        {
            if (pos.x < Console.WindowWidth && pos.x >= 0 && pos.y < Console.WindowHeight && pos.y >= 0)
            {
                if (!alt)
                {  
                    Console.SetCursorPosition(pos.x, pos.y);
                    switch (state)
                    {
                        case -1:
                            Console.Write("@");
                            break;

                        case 0:
                            Console.Write(" ");
                            break;

                        case 1:
                            Console.Write("*");
                            break;

                        case 2:
                            Console.Write("#");
                            break;

                        case 3:
                            Console.Write("~");
                            break;
                    }   
                }
                else
                {
                    Console.SetCursorPosition(pos.x, pos.y);
                    Console.Write(state);
                }
            }
        }
        static void PaintMode(Board board)
        {
            Pos pos = new Pos(0, 1);
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write("@");
            ConsoleKeyInfo key;
            int posLength = 4;
            do
            {   
                Console.SetCursorPosition(0, 0);
                posLength = $"{pos.x}; {pos.y}".Length;
                ClearSpace(posLength);
                Console.SetCursorPosition(0, 0);
                Console.Write($"{pos.x}; {pos.y}");

                key = Console.ReadKey(true);
                
                if (board.CellExists(pos)) RedrawCell(pos, 1);
                    else RedrawCell(pos, 0);
                switch(key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        pos.y = MoveMinus(pos.y);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        pos.y = MovePlus(pos.y, dimY);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        pos.x = MoveMinus(pos.x);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        pos.x = MovePlus(pos.x, dimX);
                        break;
                    case ConsoleKey.Spacebar:
                         if (board.CellExists(pos))
                            board.DestroyCellPM(pos);
                            else board.CreateCellPM(pos);
                        break;
                }
                RedrawCell(pos, -1);
            } 
            while (key.Key != (ConsoleKey.Enter));
            int state = board.CellExists(pos) ? 1 : 0; 
            RedrawCell(pos, state);
        }
        static void Run(Board board)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int delay = 0; //100
            bool paused = false;
            bool shouldContinue = true;
            do
            {
                if (!paused) shouldContinue = board.Iteration();
                Thread.Sleep(delay);
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            shouldContinue = false;
                            break;
                        case ConsoleKey.Spacebar:
                        case ConsoleKey.Enter:
                            paused = !paused;
                            Debug.WriteLineIf(paused, "Paused");
                            Debug.WriteLineIf(!paused, "Unpaused");
                            break;
                        case ConsoleKey.Add:
                        case ConsoleKey.UpArrow:
                            delay += 10;
                            Debug.WriteLine("Added delay");
                            break;
                        case ConsoleKey.Subtract:
                        case ConsoleKey.DownArrow:
                            if (delay > 0) { delay -= 10; Debug.WriteLine("Lowered delay");}
                            break;    
                    }
                } 
                
            } 
            while (shouldContinue); 
        }
    }
}
