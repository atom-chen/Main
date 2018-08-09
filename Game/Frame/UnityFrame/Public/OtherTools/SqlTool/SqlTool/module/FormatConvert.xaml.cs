using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;

namespace SqlTool.module
{
    /// <summary>
    /// Interaction logic for FormatConvert.xaml
    /// </summary>
    public partial class FormatConvert : UserControl
    {
        public FormatConvert()
        {
            InitializeComponent();
        }

        public void InitData()
        {

        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {
            if (procparam_input.Text == "")
            {
                Utility.MessageBoxNotice("str_procedure_param_need");
                return;   
            }

            string szRet = "\"";

            string[] sArray = procparam_input.Text.Replace('\r',' ').Replace('\n',' ').Split(',');
            int count = sArray.Length;
            for (int i = 0; i < count; ++i)
            {
                if(i > 0)
                {
                    szRet += ",";
                }
                string szLine = sArray[i].Trim();
                int index = szLine.IndexOf(' ');
                if (index >= 0)
                {
                    string szParam = szLine.Substring(index + 1).Trim().ToLower();
                    if (szParam.IndexOf("bigint unsigned") >= 0)
                    {
                        szRet += "%llu";
                    }
                    else if (szParam.IndexOf("bigint") >= 0)
                    {
                        szRet += "%lld";
                    }
                    else if (szParam.IndexOf("varchar(") >= 0)
                    {
                        szRet += "%s";
                    }
                    else if (szParam.IndexOf("mediumtext") >= 0)
                    {
                        szRet += "%s";
                    }
                    else if (szParam.IndexOf("text") >= 0)
                    {
                        szRet += "%s";
                    }
                    else if (szParam.IndexOf("tinyint") >= 0)
                    {
                        szRet += "%d";
                    }
                    else if (szParam.IndexOf("smallint") >= 0)
                    {
                        szRet += "%d";
                    }
                    else if (szParam.IndexOf("int unsigned") >= 0)
                    {
                        szRet += "%u";
                    }
                    else if (szParam.IndexOf("int") >= 0)
                    {
                        szRet += "%d";
                    }
                }
                else
                {
                    string szResult = Utility.GetDictionary("str_procedure_param_err", sArray[i]);
                    MessageBox.Show(szResult);
                    return;
                }
            }
            szRet += "\"";

            formatstr_output.Text = szRet;
            Utility.MessageBoxNotice("str_procedure_param_convert_over");
        }
    }
}
