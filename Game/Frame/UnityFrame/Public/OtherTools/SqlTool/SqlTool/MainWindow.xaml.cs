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
using SqlTool.module;
using Common;

namespace SqlTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqlCheck mSqlCheck = new SqlCheck();                 // 升级检查脚本生成
        public FormatConvert mFormatCovert = new FormatConvert();   // 格式化字符串转换

        public MainWindow()
        {
            InitializeComponent();
            InitUC();
        }

        public void InitUC()
        {
            TabItem switchtab = new TabItem();
            switchtab.Header = Utility.GetDictionary("str_main_sql_check");
            switchtab.Content = mSqlCheck;
            switchtab.Margin = new Thickness(0, 1, 1, 1);
            switchtab.Height = 22;
            FunctionListTab.Items.Add(switchtab);

            switchtab = new TabItem();
            switchtab.Header = Utility.GetDictionary("str_main_format_convert");
            switchtab.Content = mFormatCovert;
            switchtab.Margin = new Thickness(0, 1, 1, 1);
            switchtab.Height = 22;
            FunctionListTab.Items.Add(switchtab);
        }

        private void TabItemSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabItem item = (TabItem)FunctionListTab.SelectedItem;
                if (null == item || !(item is TabItem))
                {
                    return;
                }

                string itemvalue = item.Header.ToString();
                if ("" == itemvalue)
                {
                    return;
                }

                if (itemvalue == Utility.GetDictionary("str_main_sql_check"))
                {
                    if (mSqlCheck != null && mSqlCheck is SqlCheck)
                    {
                        mSqlCheck.InitData();
                    }
                }
                else if(itemvalue == Utility.GetDictionary("str_main_format_convert"))
                {
                    if (mFormatCovert != null && mFormatCovert is FormatConvert)
                    {
                        mFormatCovert.InitData();
                    }
                }
            }
        }
    }
}
