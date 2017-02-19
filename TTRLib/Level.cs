using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTRLib
{
    public class Level
    {
        public int LevelNumber { get; private set; }
        public char Character { get; private set; }
        public bool FirstAppearance { get; private set; }
        public List<string> Words { get; private set; }

        public string FullString { get { return Character.ToString() + " " + string.Join(" ", Words.ToArray()); } }
        
        public Level(int levelNumber, char character, bool firstAppearance, List<string> words)
        {
            LevelNumber = levelNumber;
            Character = character;
            FirstAppearance = firstAppearance;
            Words = words;
        }
    }
}
