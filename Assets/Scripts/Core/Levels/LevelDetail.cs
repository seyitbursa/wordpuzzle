using System;
using System.Collections.Generic;

namespace Core.Levels
{
    [Serializable]
    public class LevelDetail
    {
        public int LevelId { get; set; }
        public int SetId { get; set; }
        public string Letters { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public List<WordDetail> Words { get; set; }
        public List<string> OtherWords { get; set; }
        public int WordCount { get; set; }
        public double Ratio { get; set; }
        public double BoardScore { get; set; }
    }
}