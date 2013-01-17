using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleLib.Data
{
    /// <summary>
    /// This DSV Dao will take into account line breaks, field delimiters and "quote characters" inside fields.
    /// When any of those special items comes in one field, the field must start and end by "quote characters"
    /// else, it's up to us to wrap or not to wrap fields that way
    /// we allow the use of single or double quotes
    /// if we want to have quote characters in our fields, we'll escape them by doubling them: "" or ''
    /// </summary>
    public class AdvancedDsvDao: DsvDaoBase
    {
        //the Character we'll use to wrap our fields if we want them to contain line breaks or delimiters
        //we allow single or double quotes here (though I think the standard way is using only double quotes)
        protected char QuoteCharacter;

        //used for performance reasons in the DecodeField method
        private string quoteString;
        
        public AdvancedDsvDao(string path, char delimiter, bool hasHeader, char quoteCharacter = '"')
        {
            this.Path = path;
            this.Delimiter = delimiter;
            this.QuoteCharacter = quoteCharacter;
            this.quoteString = this.QuoteCharacter.ToString();
            this.HasHeader = hasHeader;
        }

        /// <summary>
        /// returns a complete line of the DSV file, taking into account that fields can contain linebreaks
        /// meaning that we'll have a "real" linebreak when the number of quote characters is even
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        protected override string GetLine(StreamReader sr)
        {
            if (sr.EndOfStream)
                return null;
            bool lineFound = false;
            string line = "";
            int marksCounter = 0;
            while (!lineFound && !sr.EndOfStream)
            {
                string curLine = sr.ReadLine();
                marksCounter += curLine.Count(it => it == this.QuoteCharacter);
                //a total even number of " indicates that the \n at hand is a real end of DSV line, not something inside a "" block
                if ( marksCounter % 2 == 0)
                {
                    lineFound = true;
                    line += curLine;
                }
                else
                    line += curLine + "\n";
            }
            return line;
        }

        /// <summary>
        /// retuns a List with the fields in one "real" line. A field can contain separators (if we wrap the field in "quote chars")
        /// so to read a complete field we'll need that the number of "quote chars" in the field is even
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected override List<string> GetFields(string line)
        {
            List<string> fields = new List<string>();
            List<string> pseudoFields = line.Split(new char[]{this.Delimiter}).ToList();
            
            string field = "";
            int marksCounter = 0;
            foreach(string pseudoField in pseudoFields)
            {
                marksCounter += pseudoField.Count(it => it == this.QuoteCharacter);
                if (marksCounter % 2 == 0)
                {
                    field += pseudoField;
                    fields.Add(this.DecodeField(field));
                    field = "";
                    marksCounter = 0;
                }
                else
                {
                    field += pseudoField + this.Delimiter;
                }
            }
            //it could be that if the fields are malformed we don't add the last one
            return fields;
        }

        /// <summary>
        /// we want to clean up the fiels in the DSV of the extra " items.
        /// "" will be replaced by "
        /// independent " will be removed
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        private string DecodeField(string st)
        {
            int curPos = 0;
            string cleanSt = "";
            while (curPos < st.Length)
            {
                if (st[curPos] == this.QuoteCharacter)
                {
                    curPos += 1;
                    if (curPos < st.Length && st[curPos] == this.QuoteCharacter)
                        cleanSt += this.QuoteCharacter;
                    //else  just add nothing here, it's a single " or '
                }
                else
                {
                    cleanSt += st[curPos];
                    curPos++;
                }
            }
            return cleanSt;
        }
        
        /// <summary>
        /// Encode fields this way:
        /// start and end with "
        /// any " in the field will be replaced by ""
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        protected override string EncodeField(string st)
        {
            //return "\"" + st.Replace("\"", "\"\"") + "\"";
            return this.quoteString
                + st.Replace(this.quoteString, this.quoteString + this.quoteString)
                + this.quoteString;
            
        }
    }
}
