using System;
using System.IO;

namespace GameOfLife
{
    static partial class Base 
    {
        static string rulesFilePath = "GameRules.dat";
        static int[] ReadRules(string filePath)
        {
            int[] rules = new int[9];
            if (!File.Exists(filePath))
            {
                using (BinaryWriter w = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    int[] defRules = {0, 0, 1, 2, 0, 0, 0, 0, 0}; 
                    foreach (int element in defRules) w.Write(element);
                }
            }
            using (BinaryReader r = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                for (int i = 0; i < 9; i++) rules[i] = r.ReadInt32();
            }
            return rules;
        }
        static void OverwriteRules(string filePath)
        {
            Console.Write("Enter the new rules (9 numbers; 0 / 3 = die, 1 / 2 = survive; 2 / 3 = create a new cell): ");
            string[] input = Console.ReadLine().Split(' ', 9);
            using (BinaryWriter w = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (string element in input) w.Write(Convert.ToInt32(element));
            }
        }        
    }
}