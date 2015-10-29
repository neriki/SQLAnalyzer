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
using System.Data;
using System.Data.Odbc;
using SQLAnalyzer.Classes.Generic;

namespace SQLAnalyzer.Classes.SqlServer
{
    public class ClassSqlServerTable : ClassTable
    {

        public override string FullName
        {
            get
            {
                return "[" + Db.Name + "].dbo.[" + Name + "]";
            }
        }

        public ClassSqlServerTable(string n, string dsn, ClassSqlServerDatabase d)
        {
            Name=n;
            Dsn = dsn;
            Db = d;
            ListField = new List<ClassField>();
        }

        public override string NbRow()
        {
            string sql = "select count(*) from " + FullName;

            using (OdbcConnection connection = new OdbcConnection(Dsn))
            {
                OdbcCommand command = new OdbcCommand(sql, connection);
                connection.Open();
                return command.ExecuteScalar().ToString();
            }
        }

        public override int LoadFromDb()
        {
            int cpt = 0;
            string sql = "use [" + Db.Name + "];exec sp_columns " + Name + ";";

            using (OdbcDataAdapter dataAdapterParam = new OdbcDataAdapter(sql, Dsn))
            {
                DataTable table = new DataTable();

                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapterParam.Fill(table);

                foreach (DataRow rowParam in table.Rows)
                {
                    cpt++;
                    ClassField fld = new ClassField(rowParam["COLUMN_NAME"].ToString(), rowParam["TYPE_NAME"].ToString(), "", "", "", "", this);
                    ListField.Add(fld);
                }
            }
            return cpt;
        }
    }
}
