﻿/*
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

namespace SQLAnalyzer.Classes.Generic
{
    public class ClassField
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string NullAccepted { get; private set; }
        public string IsKey { get; private set; }
        public string DefaultValue { get; private set; }
        public string Extra { get; private set; }

        public ClassTable Table { get; private set; }

        public ClassField(string n, string t, string nu, string k, string d, string e, ClassTable tb)
        {
            Name = n;
            Type = t;
            NullAccepted = nu;
            IsKey = k;
            DefaultValue = d;
            Extra = e;
            Table = tb;
        }
    }
}
