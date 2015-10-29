/*
SQL Anlyzer
The MIT License(MIT)

Copyright(c) 2015 Eric Boniface

Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.

*/

using System.Collections.Generic;
using System.Linq;

namespace SQLAnalyzer.Classes.Generic
{
    public abstract class ClassServer : IEnumerable<ClassDatabase>
    {
        public string Dsn { get; set; }

        protected List<ClassDatabase> Databases;

        public ClassServer() 
        {
            Databases = new List<ClassDatabase>();
        }

        public IEnumerator<ClassDatabase> GetEnumerator()
        {
            return ((IEnumerable<ClassDatabase>) Databases).GetEnumerator();
        }

        public abstract int LoadFromDb();
        

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class ClassDatabase : IEnumerable<ClassTable>
    {

        public string Name { get; protected set; }

        public ClassServer Server { get; protected set; }

        protected List<ClassTable> LstTable;

        protected string Dsn;


        public abstract int LoadFromDb();

        public IEnumerator<ClassTable> GetEnumerator()
        {
            return LstTable.OrderBy(x=>x.Name).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
