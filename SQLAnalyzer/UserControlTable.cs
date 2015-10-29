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

using System;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using SQLAnalyzer.Properties;

namespace SQLAnalyzer
{
    public partial class UserControlTable : UserControl
    {
        private BindingSource _bindingSourceTbl = new BindingSource();
        private OdbcDataAdapter _dataAdapterTbl = new OdbcDataAdapter();
        private string _tableName;
        private string _dsn;
        public FormDataServer Fds { get; private set; }

        public UserControlTable(string t, string d, FormDataServer fds)
        {
            InitializeComponent();
            _tableName = t;
            _dsn = d;
            Fds = fds;
        }

        public void LoadFromDb()
        {
            string sql="";

            dataGridViewData.DataSource = _bindingSourceTbl;
            try
            {
                String connectionString = _dsn;

                sql = "select * from " + _tableName + "";

                _dataAdapterTbl = new OdbcDataAdapter(sql, connectionString);

                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                _dataAdapterTbl.Fill(table);
                _bindingSourceTbl.DataSource = table;
                
                dataGridViewData.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception s)
            {
                MessageBox.Show(string.Format("{0}{1}\r\n{2}", Resources.UserControlTable_LoadFromDB_, sql, s));
            }
        }


    }
}
