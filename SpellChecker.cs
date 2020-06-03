using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    public static class SpellChecker
    {
        private static InputType inputMethod;
        private static BK_Tree DictionaryOfCorrectWords;
        private static List<string> TextToCheck;
        
        public static void SetInputMethod(InputType method)
        {
            inputMethod = method;
        }

        static SpellChecker()
        {
            inputMethod = new ReadFromTextFile();
            TextToCheck = new List<string>();
        }

        private static List<List<string>> ReadData()
        {
             return inputMethod.ReadData();
        }

        private static void PerformCollection(string CollectionName, IEnumerable<string> Collection)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{CollectionName}:");
            Console.ResetColor();

            foreach (string str in Collection)
            {
                Console.WriteLine($"\t{str}");
            }
        }

        public static void RunSpellChecker()
        {
            List<List<string>> Data  = ReadData();
            DictionaryOfCorrectWords = new BK_Tree(Data[0][0]);
            for (int i = 1; i < Data[0].Count; i++)
            {
                DictionaryOfCorrectWords.AddNode(Data[0][i]);
            }

            //DictionaryOfCorrectWords.PerformBKTree();
            TextToCheck = Data[1];

            foreach(string word in TextToCheck)
            {
                if(word == "\n")
                {
                    Console.Write(word);
                    continue;
                }

                Dictionary<string, int> matches = DictionaryOfCorrectWords.FindMatches(word, 2);

                if(matches.Count == 0)
                {
                    Console.Write($"{{{word}?}} ");
                }else
                if(matches.Count == 1)
                {   
                    Console.Write($"{matches.First().Key} ");
                }
                else
                if(matches.ContainsKey(word))
                {
                    Console.Write($"{word} ");
                }else
                if(matches.ContainsValue(1))
                {
                    int counter = 0;

                    string result = "";
                    foreach (var match in matches)
                        if(match.Value == 1)
                        {
                            counter++;
                            result += $"{match.Key} ";
                        }
                            
                    result = result.Substring(0, result.Length - 1);
                    if(counter == 1)
                        Console.Write($"{result} ");
                    else
                        Console.Write($"{{{result}}} ");
                }
                else
                {
                    string result = "";
                    foreach(var match in matches)
                        result += $"{match.Key} ";
                    
                    result = result.Substring(0, result.Length - 1);
                    Console.Write($"{{{result}}} ");
                }
            }
        }

        /// The method that counts Levenstein distance (Edit Distance) for two words
        public static int FindMinEditDistance(string word_A, string word_B)
        {
            word_A = " " + word_A;
            word_B = " " + word_B;

            int[,] EditDistanceArr = new int[word_B.Length, word_A.Length];

            for (int i = 0; i < word_A.Length; i++)
            {
                EditDistanceArr[0, i] = i;
            }

            for (int i = 0; i < word_B.Length; i++)
            {
                EditDistanceArr[i, 0] = i;
            }

            for (int i = 1; i < word_B.Length; i++)
            {
                for (int j = 1; j < word_A.Length; j++)
                {
                    if (word_A[j] != word_B[i])
                        EditDistanceArr[i, j] = /*Math.Min(*/Math.Min(1 + EditDistanceArr[i - 1, j],
                                                                  1 + EditDistanceArr[i, j - 1])/*,
                                                                  1 + EditDistanceArr[i - 1, j - 1])*/;
                    else
                    if (word_A[j] == word_B[i])
                        EditDistanceArr[i, j] = EditDistanceArr[i - 1, j - 1];
                }
            }

            #region 
            // just some console output for demonstration of how method works
            /* 
            Console.WriteLine($"Levenstein Distance for all substrings of {word_A} {word_B}");
            Console.Write(" \t");
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < word_A.Length; i++)
            {
                Console.Write(word_A[i] + "\t");
            }
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < word_B.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(word_B[i] + "\t");
                Console.ResetColor();
                for (int j = 0; j < word_A.Length; j++)
                {
                    Console.Write($"{EditDistanceArr[i, j]}\t");
                }
                Console.Write("\n");
            }
           */
            #endregion
            
            return EditDistanceArr[word_B.Length - 1, word_A.Length - 1];
        }
        
        public static void Perform_Finding_Levenstein_Distance()
        {
            string[] Words_A_arr = { "main", "mainly" };
            string[] Words_B_arr = { "mainy", "mainy"};

            for (int i = 0; i < Words_A_arr.Length; i++)
            {
                Console.WriteLine($"Levenstein distance between '{Words_A_arr[i]}' and '{Words_B_arr[i]}' is: " +
                              $"{FindMinEditDistance(Words_A_arr[i], Words_B_arr[i])}");
            }
        }
    }
}
