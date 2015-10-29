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
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using SQLAnalyzer.Properties;

namespace SQLAnalyzer
{
    public partial class UserControlQuery : UserControl
    {

        private BindingSource _bindingSourceTbl = new BindingSource();
        private OdbcDataAdapter _dataAdapterTbl = new OdbcDataAdapter();
        public string QueryName { get; private set; }
        private string _dsn;
        public FormDataServer Fds { get; private set; }

        private Control _hideSelection;

        public UserControlQuery(string q, string d, FormDataServer fds)
        {
            InitializeComponent();
            Fds = fds;
            _dsn = d;
            QueryName = q;
            _hideSelection = new Control();

        }

        public void LoadFromDb()
        {
            string sql = "";

            try
            {
                String connectionString = _dsn;

                sql = richTextBoxQuery.Text;

                if (sql.Trim().StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
                {

                    dataGridViewData.DataSource = _bindingSourceTbl;

                    _dataAdapterTbl = new OdbcDataAdapter(sql, connectionString);

                    DataTable table = new DataTable();
                    table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    _dataAdapterTbl.Fill(table);
                    _bindingSourceTbl.DataSource = table;

                    dataGridViewData.AutoResizeColumns(
                        DataGridViewAutoSizeColumnsMode.AllCells);
                    dataGridViewData.ReadOnly = true;
                }
                else
                {
                    dataGridViewData.DataSource = null;
                    OdbcConnection conn;
                    using (conn = new OdbcConnection(connectionString))
                    {
                        OdbcCommand cmd = new OdbcCommand(sql, conn);    
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception s)
            {
                MessageBox.Show(string.Format("{0}{1}\r\n{2}", Resources.UserControlQuery_LoadFromDB_, sql, s));
            }
        }

        //Coloration syntaxique de la RichTextBox
        private void richTextBoxQuery_TextChanged(object sender, EventArgs e)
        {
            bool hadFocus;

            if(richTextBoxQuery.Focused){
                richTextBoxQuery.HideSelection = true;
                _hideSelection.Focus();
                hadFocus = true;
            }else
                hadFocus = false;


            int selStart = richTextBoxQuery.SelectionStart;
            int selLen = richTextBoxQuery.SelectionLength;

            richTextBoxQuery.ForeColor = Color.Black;

            int cpt;

            //On colorie en bleu les mots clefs.
            string[] instructions={"select","from","where","order by", "group by","like", "update", "into","insert","set","inner","left","right","join", "on", "as"};

            foreach (string instruction in instructions) 
            {
                cpt = richTextBoxQuery.Text.ToLower().IndexOf(instruction, StringComparison.Ordinal);
                while (cpt > -1 && cpt < richTextBoxQuery.Text.Length)
                {
                    if (cpt > -1 && selLen==0)
                    {
                        richTextBoxQuery.Select(cpt, instruction.Length);
                        richTextBoxQuery.SelectionColor = Color.Blue;
                    }
                    cpt = richTextBoxQuery.Text.ToLower().IndexOf(instruction, cpt + 1, StringComparison.Ordinal);
                }
            }
            
            //on colorie en vert le texte entre quote et double quote
            string pattern = @"'(?:[^'\\]|\\.)*'";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(richTextBoxQuery.Text);

            foreach (Match match in matches)
            {
                cpt = richTextBoxQuery.Text.IndexOf(match.Value, StringComparison.Ordinal);
                while (cpt > -1 && cpt < richTextBoxQuery.Text.Length)
                {
                    if (cpt > -1 && selLen == 0)
                    {
                        richTextBoxQuery.Select(cpt, match.Value.Length);
                        richTextBoxQuery.SelectionColor = Color.Green;
                    }
                    cpt = richTextBoxQuery.Text.IndexOf(match.Value, cpt + 1, StringComparison.Ordinal);
                }
            }



            // On replace le curseur à son emplacement de départ
            richTextBoxQuery.SelectionStart = selStart;
            richTextBoxQuery.SelectionLength = selLen;
            if(selLen==0)
                richTextBoxQuery.SelectionColor = Color.Black;

            // et on récupère le focus si on l'avait.
            if (hadFocus)
                richTextBoxQuery.Focus();
            richTextBoxQuery.HideSelection = false;

        }

    }
}
