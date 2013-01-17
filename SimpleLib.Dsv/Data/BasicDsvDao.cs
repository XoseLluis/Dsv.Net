/*
 * Created by SharpDevelop.
 * User: jsampayo
 * Date: 02/11/2010
 * Time: 15:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleLib.Data
{
    /// <summary>
    /// Very Basic implementation of a DsvDao. It does not take any escaping into account, so fields can not contain line breaks or separators.
    /// Use only in the unlikely case where you're 100% sure escaping is not needed, otherwise use the AdvancedDsvDao 
    /// </summary>
    /// <typeparam name="T">the object to which the Dao will dump (using the mapper) each read row</typeparam>
    public class BasicDsvDao: DsvDaoBase
    {
        public BasicDsvDao(string path, char separator, bool hasHeader)
        {
            this.Path = path;
            this.Delimiter = separator;
            this.HasHeader = hasHeader;
        }

        protected override string GetLine(StreamReader sr)
        {
            if (sr.EndOfStream)
                return null;
            else
                return sr.ReadLine();
        }

        protected override List<string> GetFields(string line)
        {
            return line.Split(new char[] { this.Delimiter }).ToList();
        }

        protected override string EncodeField(string st)
        {
            return st;
        }
    }
}
