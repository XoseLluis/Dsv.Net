using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleLib.Data
{
    /// <summary>
    /// Class to deal with Delimiter-Separated Values files (CSV, TSV...)
    /// http://en.wikipedia.org/wiki/Delimiter-separated_values
    /// </summary>
    public abstract class DsvDaoBase
    {
        protected string Path;
        protected char Delimiter;
        public bool HasHeader;
        
        //GetLine takes care of checking if we're at the end of the Stream, and return null in that case
        protected abstract string GetLine(StreamReader sr);
        
        protected abstract List<string> GetFields(string line);

        protected abstract string EncodeField(string field);
       
		public List<string> GetHeaders()
		{
            List<string> headers = null;
			if (this.HasHeader)
			{
                using (StreamReader sr = new StreamReader(this.Path))
				{
                    string line = this.GetLine(sr);
                    headers = this.GetFields(line);
				}
				
			}
			return headers;
		}

		//return an IEnumerable for performance reasons (we keep the CSV open and return line by line)
        public IEnumerable<List<string>> GetDataEntries()
        {
            using (StreamReader sr = new StreamReader(this.Path))
            {
                if (this.HasHeader)
                    if (this.GetLine(sr) == null)
                        yield break;

                string line = this.GetLine(sr);
                while (line != null)
                {
                    yield return this.GetFields(line);
                    line = this.GetLine(sr);
                }
            }
        }
        
        /// <summary>
        /// appends entry to the file
        /// </summary>
        /// <param name="entry"></param>
        public void AddDataEntry(IEnumerable<string> entry)
        {
            entry = entry.Select(field => this.EncodeField(field)).ToList();
            using (StreamWriter sw = new StreamWriter(this.Path, true))
            {
                //as I'm not chaining this to other Linq queries, I think String.Join is a bit more readable here
                //sw.WriteLine(entry.Aggregate((it1, it2) => it1 + this.Delimiter + it2));
                sw.WriteLine(String.Join(this.Delimiter.ToString(), entry));
            }
        }
        
        /// <summary>
        /// appends entries to the file (though intended for Data Entries, it could be used for adding both headers and data entries)
        /// we provide this method in addition to AddDataEntry for performance reasons,
        /// for adding several entries it's better to call this method than calling
        /// AddDataEntry in a loop (opening n times the stream...)
        /// </summary>
        /// <param name="entries"></param>
        public void AddDataEntries(IEnumerable<IEnumerable<string>> entries)
        {
            using (StreamWriter sw = new StreamWriter(this.Path, true))
            {
	            foreach(var entry in entries)
	                sw.WriteLine(String.Join(this.Delimiter.ToString(), 
                        entry.Select(field => this.EncodeField(field))
                    ));
            }
        }

        /// <summary>
        /// Removes all Data Entries. If file had Headers these are kept
        /// </summary>
        public void RemoveDataEntries()
        {
            string newContent = "";
            if (this.HasHeader)
            {
                newContent = this.GetHeaders()
                    .Select(header => this.EncodeField(header))
                    .Aggregate((h1, h2) => h1 + this.Delimiter + h2);
            }
            //overwrite file with just the headers
            using (StreamWriter st = new StreamWriter(this.Path))
            {
                st.WriteLine(newContent);
            }
        }

        /// <summary>
        /// removes headers from the file, keeping the data entries as they are
        /// </summary>
        public void RemoveHeaders()
        {
            if (this.HasHeader)
            {
                //overwrite file with current contente excepting headers
                string dataContent = this.GetDataEntries()
                    .Select(entry => entry.Select(field => this.EncodeField(field)))
                    .Select(e1 => e1.Aggregate((f1, f2) => f1 + this.Delimiter + f2))
                    .Aggregate((l1, l2) => l1 + "\n" + l2);
                using (StreamWriter st = new StreamWriter(this.Path))
                {
                    st.WriteLine(dataContent);
                }
                this.HasHeader = false;
            }
        }

        public void UpdateHeaders(IEnumerable<string> headers)
        {
            string dataContent = this.GetDataEntries()
                .Select(entry => entry.Select(field => this.EncodeField(field)))
                .Select(e1 => e1.Aggregate((f1, f2) => f1 + this.Delimiter + f2))
                .Aggregate((l1, l2) => l1 + "\n" + l2);
            using (StreamWriter st = new StreamWriter(this.Path))
            {
                st.WriteLine(headers
                    .Select(header => this.EncodeField(header))
                    .Aggregate((h1, h2) => h1 + this.Delimiter + h2));
                st.WriteLine(dataContent);
            }
            this.HasHeader = true;
        }
    }
}

    