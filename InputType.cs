using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SpellChecker
{
    public interface InputType
    {
        List<List<string>> ReadData();
        
    }

    public class ReadFromTextFile : InputType
    {
        private string Path;
        
        public ReadFromTextFile()
        {
            string parentDir = Directory.GetCurrentDirectory();
            parentDir = Directory.GetParent(parentDir).FullName;
            parentDir = Directory.GetParent(parentDir).FullName;

            Path = System.IO.Path.Combine(parentDir, @"Resources\\Input.txt");
        }

        public List<List<string>> ReadData()
        {
            List<List<string>> Result = new List<List<string>>(2);
            List<string> ListForDictionary =  new List<string>();
            List<string> ListForTextToCheck = new List<string>();

            using (StreamReader ReadingStream =
                   new StreamReader(Path, System.Text.Encoding.Default))
            {
                bool DictFilled = false;
                string TextLine;
                while ((TextLine = ReadingStream.ReadLine()) != null)
                {
                    if(TextLine == "===" && !DictFilled)
                    {
                        DictFilled = true;
                        continue;
                    }

                    if (TextLine.Length != 0)
                    {
                        List<string> EditedTextLine = StringTool.GetWords(TextLine, StringTool.Mode.w);
                        if (!DictFilled)
                            ListForDictionary.AddRange(EditedTextLine);
                        else
                        {
                            ListForTextToCheck.AddRange(EditedTextLine);
                            if (EditedTextLine != null && EditedTextLine.Count != 0)
                                ListForTextToCheck.Add("\n");
                        }
                    }
                }
            }
            Result.Add(ListForDictionary);
            Result.Add(ListForTextToCheck);
            return Result;
        }
    }
    #region
    /*
    public class ReadFromConsole : InputType
    {
        public List<List<string>> ReadData()
        {
            List<List<string>> Result = new List<List<string>>();
            Result[0] = new List<string>();

            string TextLine;
            while (true)
            {
                TextLine = Console.ReadLine();
                string[] EditedTextLine = StringEditor.Edit(TextLine);

                foreach (string word in EditedTextLine)
                {
                    if (word == "===")
                            return Result;
                    Result.Add(word);
                }
                //Result.Add("\n");
            }
            //return Result;
        }
    }

      /*  public class ReadFromBinaryFile : InputType
        {
            public List<string> ReadData()
            {

            }
        }
        */
    #endregion
    static class StringTool
    {
        public static string[] SplitByChars(string InputString)
        {
            char[] CharSeparators = new char[] 
            { ' ', '.', ',', '\n', ';', '-', ':', '[', ']',
              '{', '}', '(', ')', '!', '?', '&', '/', '|', '\n'};

            string[] Result = InputString.Split(CharSeparators, 
                StringSplitOptions.RemoveEmptyEntries);

            return Result;
        }

        public enum Mode
        {
            w, // for any alphabetical-digital word
            W  // for any other words
        }
        
        /// finds all words in a string
        public static List<string> GetWords(string InputString, Mode mode)
        {
            List<string> Result = new List<string>();
            MatchCollection matchedWords = Regex.Matches(InputString, $@"\b[\{mode}]*\b");
            foreach(Match elem in matchedWords)
            {
                if(elem.Value.Length != 0 && elem.Value != null)
                    Result.Add(elem.Value);
            }
            return Result;
        }
    }
}
