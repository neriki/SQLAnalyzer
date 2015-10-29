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
using System.Collections.Generic;
using System.Windows.Forms;
using SQLAnalyzer.Classes.Generic;
using SQLAnalyzer.Classes.MySQL;
using SQLAnalyzer.Classes.SqlServer;

namespace SQLAnalyzer
{
    public partial class FormDataServer : Form
    {
        private ClassServer _oDatabase;
        public Dictionary<string, TabPage> LstPage { get; private set; }
        private string _queryBaseName = "Query ";

        public FormDataServer()
        {
            InitializeComponent();
            LstPage = new Dictionary<string, TabPage>();
        }

        public void LoadFromDb(string dsn, DbType type)
        {
            try {
                switch (type)
                {
                    case DbType.MySql:
                        _oDatabase = new ClassMySqlServer();
                        break;
                    case DbType.SqlServer:
                        _oDatabase = new ClassSqlServerServer();
                        break;
                }

                Text = dsn;

                _oDatabase.Dsn = dsn;

                _oDatabase.LoadFromDb();

                treeViewDB.Nodes.Clear();

                foreach (ClassDatabase oDb in _oDatabase)
                {
                    TreeNode tn = treeViewDB.Nodes.Add(oDb.Name);
                    tn.Tag = oDb;
                    oDb.LoadFromDb();
                    foreach (ClassTable oDt in oDb)
                    {
                        //tn.Nodes.Add(oDt.name + " (" + oDt.nbRow() + " lignes )");
                        TreeNode tt = tn.Nodes.Add(oDt.Name);
                        tt.Tag = oDt;
                        oDt.LoadFromDb();
                        foreach (ClassField oFd in oDt)
                        {
                            TreeNode tf = tt.Nodes.Add(oFd.Name);
                            tf.Tag = oFd;
                            if (oFd.Type != "") tf.Nodes.Add(oFd.Type);
                            if (oFd.NullAccepted != "") tf.Nodes.Add(oFd.NullAccepted);
                            if (oFd.IsKey != "") tf.Nodes.Add(oFd.IsKey);
                            if (oFd.Extra != "") tf.Nodes.Add(oFd.Extra);

                        }
                    }
                }
            }
            catch(Exception e)
            {
                Close();
                throw (e);
            }
        }

        private void treeViewDB_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeViewDB.SelectedNode;

            //Si on est dans une table.
            
            //if(node.Level==1) 
            //if(node.Tag.GetType() == typeof(ClassTable)) //Ne marche plus pour les types dérive...
            if (node != null && node.Tag is ClassTable)
            {
                ClassTable otb=(ClassTable)node.Tag;
                //string tableFullName = "`" + node.Parent.Text + "`.`" + node.Text + "`";
                string tableFullName = otb.FullName;
                if (tabControlDataServer.TabPages.ContainsKey(tableFullName))
                {
                    tabControlDataServer.SelectedTab=tabControlDataServer.TabPages[tableFullName];
                }else
                {
                    TabPage tb = new TabPage();
                    
                    tabControlDataServer.TabPages.Add(tb);
                    UserControlTable tbl = new UserControlTable(tableFullName, _oDatabase.Dsn, this);
                    tb.Text = tableFullName;
                    tb.Name = tableFullName;
                    tb.Controls.Add(tbl);
                    tbl.Dock = DockStyle.Fill;
                    tbl.LoadFromDb();
                    tabControlDataServer.SelectedTab = tb;

                }

                
            }




            //MessageBox.Show(node.FullPath.ToString() + "\r\n" + node.Level);

        }

        public void CloseTab(string name)
        {
            tabControlDataServer.TabPages.Remove(tabControlDataServer.TabPages[name]);   
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            tabControlDataServer.TabPages.Remove(tabControlDataServer.SelectedTab);   
        }

        private void tabControlDataServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pour cacher et afficher le bouton execute
        }

        private void toolStripButtonQuery_Click(object sender, EventArgs e)
        {
            TabPage tb = new TabPage();
            string queryName = NextQueryName();

            tabControlDataServer.TabPages.Add(tb);
            UserControlQuery tbl = new UserControlQuery(queryName, _oDatabase.Dsn, this);
            tb.Text = queryName;
            tb.Name = queryName;
            tb.Controls.Add(tbl);
            tbl.Dock = DockStyle.Fill;
            //tbl.LoadFromDB();
            tabControlDataServer.SelectedTab = tb;
            tb.Tag = tbl;
        }

        private string NextQueryName()
        {
            int queryNb=1;

            while (tabControlDataServer.TabPages.ContainsKey(_queryBaseName + queryNb)) queryNb++;

            return _queryBaseName + queryNb;
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            UserControlQuery tbl = (UserControlQuery)(tabControlDataServer.SelectedTab.Tag);
            tbl.LoadFromDb();
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
