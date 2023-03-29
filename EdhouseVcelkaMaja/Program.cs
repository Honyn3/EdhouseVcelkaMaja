using System;
using System.Collections.Generic;
using System.IO;

namespace EdhouseVcelkaMaja
{
    internal class Program
    {
        static Tree[,] map;
        static int visibleCount = 0;

        static void Main(string[] args)
        {
            if (GetTreesFromFile()) //True pokud se načte les ze souboru
            {
                //Algoritmy, které projedou les ze všech stran
                LineFromLeft();
                LineFromRight();
                LineFromBottom();
                LineFromTop();

                //ShowVisible(); //Metoda vykreslí mapu lesa s viditelnými stromy
                CountVisible();
            }

            Console.ReadKey();
        }

        static void LineFromLeft()
        {
            int maxHeight = map[1, 0].Height; //Zjistí se nejvyšší hodnota v prvním řádku a nultém sloupci
            for (int i = 1; i < map.GetLength(0) - 1; i++) //Projedou se všechny řádky
            {
                for (int j = 1; j < map.GetLength(1) - 1; j++) //V řádcích se projedou všechny sloupce
                {
                    if (map[i, j].Height > maxHeight) //Pokud má vybraný strom větší výšku než je maximalní ve sloupci
                    {
                        map[i, j].visible = true; //Označí se za viditelný
                        maxHeight = map[i, j].Height; //Maximální výška se přepíše na výšku tohoto stromu
                        if (maxHeight == 9) { break; }; //Pokud je výška 9, loop se přeruší
                    }
                }
                maxHeight = map[i + 1, 0].Height; //Po každém sloupci se zjistí nová nejvyšší hodnota na začátku dalšího řádku
            }
        }

        //Další metody fungují analogicky k metodě LineFromLeft
        //Mění se akorát směr, odkud algoritmus checkuje výšku
        static void LineFromRight()
        {
            int maxHeight = map[1, map.GetLength(1) - 1].Height;
            for (int i = map.GetLength(0) - 2; i > 0; i--)
            {
                for (int j = map.GetLength(1) - 2; j > 0; j--)
                {
                    if (map[i, j].Height > maxHeight)
                    {
                        map[i, j].visible = true;
                        maxHeight = map[i, j].Height;
                        if (maxHeight == 9) break;
                    }
                }
                maxHeight = map[i - 1, map.GetLength(1) - 1].Height;
            }
        }

        static void LineFromBottom()
        {
            int maxHeight = map[map.GetLength(0) - 1, 1].Height;
            for (int i = map.GetLength(1) - 2; i > 0; i--)
            {
                for (int j = map.GetLength(0) - 2; j > 0; j--)
                {
                    if (map[j, i].Height > maxHeight)
                    {
                        map[j, i].visible = true;
                        maxHeight = map[j, i].Height;
                        if (maxHeight == 9) break;
                    }
                }
                maxHeight = map[map.GetLength(0) - 1, i - 1].Height;
            }
        }

        static void LineFromTop()
        {
            int maxHeight = map[1, 1].Height;
            for (int i = map.GetLength(1) - 2; i > 0; i--)
            {
                for (int j = 1; j < map.GetLength(0) - 2; j++)
                {
                    if (map[j, i].Height > maxHeight)
                    {
                        map[j, i].visible = true;
                        maxHeight = map[j, i].Height;
                        if (maxHeight == 9) break;
                    }
                }
                maxHeight = map[0, i - 1].Height;
            }
        }


        static bool GetTreesFromFile()
        {
            List<int[]> list = new List<int[]>();

            string path;
            if (File.Exists("..\\..\\..\\mapa.txt"))
            {
                path = "..\\..\\..\\mapa.txt";
            }
            else if (File.Exists("mapa.txt"))
            {
                path = "mapa.txt";
            }
            else
            {
                Console.WriteLine("Soubor nenalezen");
                return false;
            }

            using (StreamReader sr = new StreamReader(path)) //Najde se soubor
            {
                while (!sr.EndOfStream)
                {
                    //Soubor se přepíše do listu
                    string line = sr.ReadLine();
                    int[] trees = new int[line.Length];
                    for (int i = 0; i < line.Length; i++)
                    {
                        trees[i] = line[i] - '0'; //String čísla se přepíše na int
                        if (trees[i] < 0 || trees[i] > 9)
                        {
                            Console.WriteLine("Soubor obsahuje neplatné znaky");
                            return false;
                        }
                    }
                    list.Add(trees);
                }
            }

            //List se přepíše do array
            map = new Tree[list.Count, list[0].Length];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    map[i, j] = new Tree(list[i][j]);
                }
            }

            visibleCount += 2 * map.GetLength(0) + 2 * (map.GetLength(1) - 2); //Připočtě se počet stromů na okrajích

            return true;
        }

        static void ShowVisible()
        {
            for (int i = 0; i < map.GetLength(0); i++) //Projede se celý les a spočítají se viditelné stromy
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].visible || i == 0 || j == 0 || j == map.GetLength(1) - 1 || i == map.GetLength(0) - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(map[i, j].Height);
                }
                Console.WriteLine();
            }
        }

        static void CountVisible()
        {
            for (int i = 0; i < map.GetLength(0); i++) //Projede se celý les a spočítají se viditelné stromy
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].visible)
                    {
                        visibleCount++;
                    }
                }
            }
            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine("Je viditelných " + visibleCount + " stromů");
        }
    }

    internal class Tree //Třída les na uložení výšky a viditelnosti stromu
    {
        public int Height;
        public bool visible;

        public Tree(int height)
        {
            Height = height;
            visible = false; //Všechny stromu nejsou viditelné, dokud není dokázán opak
        }
    }
}
