using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core.Levels
{
    public class TextLevelReader : MonoBehaviour, ILevelReader
    {
        public List<LevelDetail> ReadLevels(string input)
        {
            Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] lines = input.Split("\n"[0]);

            List<LevelDetail> levelDetails = new List<LevelDetail>();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] row = regex.Split(lines[i]);
                LevelDetail levelInformation = new LevelDetail
                {
                    LevelId = Convert.ToInt32(row[0]),
                    SetId = Convert.ToInt32(row[1]),
                    Letters = row[2].Replace("\"", ""),
                    Row = Convert.ToInt32(row[3]),
                    Column = Convert.ToInt32(row[4]),
                    OtherWords = string.IsNullOrWhiteSpace(row[6]) ? new List<string>() : new List<string>(row[6].Replace("\"", "").Split(',')),
                    WordCount = Convert.ToInt32(row[7]),
                    Ratio = Convert.ToDouble(row[8]),
                    BoardScore = Convert.ToDouble(row[9]),
                };

                string[] words = row[5].Split('|');
                List<WordDetail> wordDetails = new List<WordDetail>();
                foreach (var word in words)
                {
                    string[] wordInformationArray = regex.Split(word.Replace("\"", ""));
                    wordDetails.Add(new WordDetail
                    {
                        Row = Convert.ToInt32(wordInformationArray[0]),
                        Column = Convert.ToInt32(wordInformationArray[1]),
                        Word = wordInformationArray[2],
                        Orientation = wordInformationArray[3]
                    });
                }
                levelInformation.Words = wordDetails;
                levelDetails.Add(levelInformation);
            }

            return levelDetails;
        }
    }
}