using System;

namespace Core.Levels
{
    [Serializable]
    public class WordDetail
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Word { get; set; }
        public string Orientation { get; set; }
    }
}