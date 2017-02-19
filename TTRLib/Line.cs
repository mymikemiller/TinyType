using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTRLib
{
    public class Line
    {
        private int mLastCorrectIndex = -1;
        private bool mLastLetterIncorrect = false;

        public string Text { get; private set; }
        public string CorrectText { get { return Text.Substring(0, mLastCorrectIndex + 1); } }
        public string IncorrectText { get { return mLastLetterIncorrect ? Text[mLastCorrectIndex + 1].ToString() : ""; } }
        public string NextText { get { return Text.Substring(mLastCorrectIndex + 1); } }

        public Line(string text)
        {
            Text = text;
        }

        public void TypeLetter(char letter)
        {
            if (char.ToLower(letter) == char.ToLower(Text[mLastCorrectIndex + 1]))
            {
                mLastCorrectIndex++;
                mLastLetterIncorrect = false;
            }
            else
            {
                mLastLetterIncorrect = true;
            }
        }
    }
}
