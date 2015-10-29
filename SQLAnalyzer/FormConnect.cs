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
using System.Windows.Forms;
using SQLAnalyzer.Classes.Generic;
using SQLAnalyzer.Classes.MySQL;
using SQLAnalyzer.Classes.SqlServer;

namespace SQLAnalyzer
{
    public partial class FormConnect : Form
    {
        
        private FormMain _frmMain;
        private ClassConnectString _cs;
        private DbType _db;

        public FormConnect(FormMain f)
        {
            InitializeComponent();
            _frmMain = f;

            comboBoxDatabaseType.DataSource = Enum.GetValues(typeof (DbType));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                FormDataServer newMdiChild = new FormDataServer();
                // Set the Parent Form of the Child window.
                newMdiChild.MdiParent = _frmMain;
                // Display the new form.
                newMdiChild.Show();

                newMdiChild.LoadFromDb(_cs.ConnectString, _db);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void comboBoxDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Enum.TryParse(comboBoxDatabaseType.SelectedValue.ToString(), out _db);

                switch (_db)
                {
                    case DbType.MySql:
                        _cs = new ClassConnectStringMySql();
                        break;
                    case DbType.SqlServer:
                        _cs = new ClassConnectStringSqlServer();
                        break;
                    default:
                        throw new Exception("Please select a database type!");
                }

                _cs.User = textBoxLogin.Text;
                _cs.Password = textBoxPassword.Text;
                _cs.Server = textBoxServer.Text;

                textBoxConnectString.Text = _cs.ConnectString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxServer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _cs.Server = textBoxServer.Text;
                textBoxConnectString.Text = _cs.ConnectString;
            }
            catch
            {
                // ignored
            }
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _cs.User = textBoxLogin.Text;
                textBoxConnectString.Text = _cs.ConnectString;
            }
            catch
            {
                // ignored
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _cs.Password = textBoxPassword.Text;
                textBoxConnectString.Text = _cs.ConnectString;
            }
            catch
            {
                // ignored
            }
        }


    }
}
