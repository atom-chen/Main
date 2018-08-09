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
using System.IO;

namespace SqlTool.module
{
    // 列的数据结构
    public class ColumnData
    {
        public string mType;       // 列的类型
        public string mDefValue;   // 列的默认值
        public int mCrc;        // 列的CRC校验值

        public void SetCrc()
        {
            mCrc = Utility.GetCRC32(mType);
            mCrc += Utility.GetCRC32(mDefValue);
        }
    }

    // 表的索引属性
    public class IndexData
    {
        public List<String> mCols = new List<string>();       // 索引涉及的列
        public bool mIsUnique;                                // 是否唯一索引
        public int mCrc;                                      // 该索引的CRC
    }

    // 表的数据结构
    public class TableAttr
    {
        public Dictionary<string, ColumnData> mColumns = new Dictionary<string, ColumnData>();      // 表中的列
        public int mCrcCol;                                                                         // 列的CRC
        public List<String> mPrimaryKey = new List<string>();                                       // 表的主键
        public int mPrimaryCrc;                                                                     // 表的主键的CRC
        public Dictionary<string, IndexData> mIndexs = new Dictionary<string, IndexData>();         // 表的索引
        public int mCrcIndex;                                                                       // 索引的CRC
       
        // 获取表的CRC值
        public int GetCrc()
        {
            mCrcCol = 0;
            foreach (KeyValuePair<string, ColumnData> sc in mColumns)
            {
                mCrcCol += sc.Value.mCrc;
            }

            mCrcIndex = 0;
            foreach (KeyValuePair<string, IndexData> si in mIndexs)
            {
                mCrcIndex += si.Value.mCrc;
            }

            return mCrcCol + mPrimaryCrc + mCrcIndex;
        }
    }

    // 存储过程与函数的参数属性
    public class ParamAttr
    {
        public string mName;        // 参数名字
        public string mType;        // 参数类型
    }

    // 存储过程数据结构
    public class ProcData
    {
        public List<ParamAttr> mParams = new List<ParamAttr>(); // 参数列表
        public List<string> mLines = new List<string>();     // 存储过程的行列表（在begin 和 end 之间的行数）
        public int mCrc;             // 存储过程的crc值
    }

    // 函数的数据结构
    public class FuncData
    {
        public List<ParamAttr> mParams = new List<ParamAttr>(); // 参数列表
        public string mRetVal;          // 返回类型
        public List<string> mLines = new List<string>();     // 行列表（在begin 和 end 之间的行数）
        public int mCrc;             // crc值
    }

    // 更新检查时需要的索引数据
    public class IndexCheckData
    {
        public string mTable;       // 表名
        public List<string> mCols = new List<string>();  // 列名
    }


    /// <summary>
    /// Interaction logic for SqlCheck.xaml
    /// </summary>
    public partial class SqlCheck : UserControl
    {
        // 升级脚本所需数据结构
        public Dictionary<string, int> mTables;                 // 有改动的表
        public Dictionary<string, List<string>> mAddColumns;    // 表增加的列（ table ——> columns_list）
        public Dictionary<string, List<string>> mDelColumns;    // 表删除的列
        public Dictionary<string, string> mDelIndex;            // 表删除的索引(iname——>tname)
        public Dictionary<string, IndexCheckData> mCheckIndex;  // 需要检查的新增的索引信息
        public List<string> mProcedures;                        // 需要检查的存储过程
        public List<string> mFuncs;                             // 需要检查的函数
        public int mVersion;
        public string mDbName;

        // 升级脚本需要的数据结构
        public Dictionary<string, TableAttr> mOldTables;      // 旧的建库脚本的表
        public Dictionary<string, ProcData> mOldProcs;         // 旧的建库脚本的存储过程
        public Dictionary<string, FuncData> mOldFuncs;         // 旧的建库脚本的函数

        public Dictionary<string, TableAttr> mNewTables;      // 新的建库脚本的表
        public Dictionary<string, ProcData> mNewProcs;         // 新的建库脚本的存储过程
        public Dictionary<string, FuncData> mNewFuncs;         // 新的建库脚本的函数

        public SqlCheck()
        {
            InitializeComponent();
        }

        public void InitData()
        {
            mTables = new Dictionary<string, int>();
            mAddColumns = new Dictionary<string, List<string>>();
            mDelColumns = new Dictionary<string, List<string>>();
            mDelIndex = new Dictionary<string, string>();
            mCheckIndex = new Dictionary<string, IndexCheckData>();
            mProcedures = new List<string>();
            mFuncs = new List<string>();
            mVersion = 0;
            mDbName = "";

            mOldTables = new Dictionary<string, TableAttr>();
            mOldProcs = new Dictionary<string, ProcData>();
            mOldFuncs = new Dictionary<string, FuncData>();

            mNewTables = new Dictionary<string, TableAttr>();
            mNewProcs = new Dictionary<string, ProcData>();
            mNewFuncs = new Dictionary<string, FuncData>();
        }

        private void oldselect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".sql";
            dlg.Filter = "SQL documents (.sql)|*.sql";

            Nullable<bool> result = dlg.ShowDialog();
            if( result == true)
            {
                oldpath.Text = dlg.FileName;
            }
        }

        private void newselect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".sql";
            dlg.Filter = "SQL documents (.sql)|*.sql";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                newpath.Text = dlg.FileName;
            }
        }

        private void upselect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".sql";
            dlg.Filter = "SQL documents (.sql)|*.sql";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                uppath.Text = dlg.FileName;
            }
        }

        // 日志信息输出
        private void MsgOutPut(string msg)
        {
            resultmsg.Text += msg;
            resultmsg.Text += '\n';
        }

        // 建库脚本读取
        private bool read_init_script(string filename, Dictionary<string, TableAttr> tables, Dictionary<string, ProcData> procs, Dictionary<string, FuncData> funcs)
        {
            StreamReader sr = new StreamReader(filename, Encoding.UTF8);
            string line;
            int linenum = 0;
            int index = -1;

            while(!sr.EndOfStream)
            {
                line = sr.ReadLine().Trim();
                linenum++;
                if (line == "" || line[0] == '#' || (line[0] == '/' && line[1] == '*'))
                {
                    continue;           // 注释略过
                }

                if(line.ToLower().IndexOf("use ") >= 0)
                {
                    // 数据库名字
                    line = line.Substring(4).Trim();
                    index = line.IndexOf(';');
                    if (index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                        sr.Close();
                        return false;
                    }
                    mDbName = line.Substring(0, index).Trim();
                }
                else if (line.ToLower().IndexOf("create table") >= 0)
                {
                    index = line.LastIndexOf(' ');
                    if(index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                        sr.Close();
                        return false;
                    }
                    string tname = line.Substring(index).Trim();
                    TableAttr tmpTable = new TableAttr();

                    while ((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if ( line.IndexOf(';') >= 0)
                        {
                            // 表解析结束
                            break;
                        }
                        if (line.ToLower().IndexOf("not null") >= 0)
                        {
                            // 列解析
                            ColumnData tmpCol = new ColumnData();

                            index = line.IndexOf(' ');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            string szColName = line.Substring(0, index);
                            line = line.Substring(index + 1).Trim();
                            index = line.ToLower().IndexOf("not null");
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            tmpCol.mType = line.Substring(0, index).Trim();
                            index = line.ToLower().IndexOf("default");
                            if (index < 0 && szColName != "aid")
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            if(szColName != "aid")
                            {
                                line = line.Substring(index + 7).Trim();
                                index = line.IndexOf(',');
                                if (index < 0)
                                {
                                    MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                    sr.Close();
                                    return false;
                                }
                                tmpCol.mDefValue = line.Substring(0, index);
                            }
                            else
                            {
                                tmpCol.mDefValue = "1";
                            }

                            tmpCol.SetCrc();
                            tmpTable.mColumns.Add(szColName, tmpCol);
                        }
                        else if (line.ToLower().IndexOf("primary key") >= 0)
                        {
                            // 解析主键
                            index = line.IndexOf('(');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            line = line.Substring(index + 1).Trim();
                            string szPK = "";
                            int nCrc = 0;
                            while ((index = line.IndexOf(',')) >= 0 && line[index-1] != ')')
                            {
                                szPK = line.Substring(0, index).Trim();
                                tmpTable.mPrimaryKey.Add(szPK);
                                nCrc += Utility.GetCRC32(szPK);
                                line = line.Substring(index + 1).Trim();
                            }
                            index = line.IndexOf(')');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            szPK = line.Substring(0, index);
                            tmpTable.mPrimaryKey.Add(szPK);
                            nCrc += Utility.GetCRC32(szPK);
                            tmpTable.mPrimaryCrc = nCrc;
                        }
                        else if ( line.ToLower().IndexOf("index ") >= 0)
                        {
                            // 解析索引
                            IndexData tmpIndex = new IndexData();

                            if(line.ToLower().IndexOf("unique index") >= 0)
                            {
                                tmpIndex.mIsUnique = true;
                                line = line.Substring(12).Trim();
                            }
                            else
                            {
                                tmpIndex.mIsUnique = false;
                                line = line.Substring(5).Trim();
                            }

                            index = line.IndexOf('(');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            string szIName = line.Substring(0, index);
                            string szCol = "";
                            int nCrc = 0; 
                            line = line.Substring(index + 1).Trim();
                            while ((index = line.IndexOf(',')) >= 0 && line[index - 1] != ')')
                            {
                                szCol = line.Substring(0, index).Trim();
                                tmpIndex.mCols.Add(szCol);
                                nCrc += Utility.GetCRC32(szCol);
                                line = line.Substring(index + 1);
                            }
                            index = line.IndexOf(')');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            szCol = line.Substring(0, index);
                            tmpIndex.mCols.Add(szCol);
                            nCrc += Utility.GetCRC32(szCol);
                            nCrc += Utility.GetCRC32(tmpIndex.mIsUnique.ToString());
                            tmpIndex.mCrc = nCrc;
                            tmpTable.mIndexs.Add(szIName, tmpIndex);
                        }
                    }

                    tables.Add(tname, tmpTable);
                }
                else if (line.ToLower().IndexOf("create index") >= 0 || line.ToLower().IndexOf("create unique index") >= 0)
                {
                    // 解析索引
                    IndexData tmpIndex = new IndexData();
                    if (line.ToLower().IndexOf("create index") >= 0)
                    {
                        line = line.Substring(12).Trim();
                        tmpIndex.mIsUnique = false;
                    }
                    else
                    {
                        line = line.Substring(19).Trim();
                        tmpIndex.mIsUnique = true;
                    }
                    
                    index = line.IndexOf(' ');
                    if (index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                        sr.Close();
                        return false;
                    }
                    string szIname = line.Substring(0, index);
                    index = line.ToLower().IndexOf("on ");
                    if (index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                        sr.Close();
                        return false;
                    }
                    string szTName = line.Substring(index + 3).Trim();
                    if (!tables.ContainsKey(szTName))
                    {
                        MsgOutPut(Utility.GetDictionary("str_up_table_index_err", filename, szIname, szTName, linenum));
                        sr.Close();
                        return false;
                    }
                    
                    int nCrc = 0;
                    while ((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if(line == "(" || line == ")")
                        {
                            continue;
                        }
                        if (line.IndexOf(';') >= 0)
                        {
                            break;
                        }

                        if((index = line.IndexOf(',')) >= 0)
                        {
                            string szCol = line.Substring(0, index).Trim();
                            tmpIndex.mCols.Add(szCol);
                            nCrc += Utility.GetCRC32(szCol);
                        }
                        else
                        {
                            tmpIndex.mCols.Add(line);
                            nCrc += Utility.GetCRC32(line);
                        }
                    }
                    nCrc += Utility.GetCRC32(tmpIndex.mIsUnique.ToString());
                    tmpIndex.mCrc = nCrc;
                    tables[szTName].mIndexs.Add(szIname, tmpIndex);
                }
                else if(line.ToLower().IndexOf("create procedure") >= 0)
                {
                    // 解析存储过程
                    line = line.Substring(16).Trim();
                    string szProcName = "";

                    index = line.IndexOf('(');
                    if (index >= 0)
                    {
                        szProcName = line.Substring(0, index).Trim();
                    }
                    else
                    {
                        szProcName = line;
                    }
                    
                    int nCrc = 0;
                    ProcData tmpProc = new ProcData();

                    // 参数解析
                    while ((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if (line.ToLower().IndexOf("begin") >= 0)
                        {
                            break;
                        }
                        ParamAttr tmpParma = new ParamAttr();

                        index = line.IndexOf(' ');
                        if (index < 0)
                        {
                            if(line == ")" || line == "(")
                            {
                                continue;
                            }
                            MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                            sr.Close();
                            return false;
                        }
                        tmpParma.mName = line.Substring(0, index).Trim();
                        nCrc += Utility.GetCRC32(tmpParma.mName);
                        line = line.Substring(index + 1).Trim();
                        if ((index = line.IndexOf(',')) >= 0)
                        {
                            tmpParma.mType = line.Substring(0, index).Trim();
                        }
                        else
                        {
                            tmpParma.mType = line;
                        }

                        if (tmpParma.mType == "")
                        {
                            MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                            sr.Close();
                            return false;
                        }

                        nCrc += Utility.GetCRC32(tmpParma.mType);
                        tmpProc.mParams.Add(tmpParma);
                    }

                    // 解析函数体
                    while((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if (line.ToLower().IndexOf("end//") >= 0 || line.ToLower().IndexOf("end;//") >= 0)
                        {
                            break;
                        }
                        tmpProc.mLines.Add(line);
                        nCrc += Utility.GetCRC32(line);
                    }

                    tmpProc.mCrc = nCrc;
                    procs.Add(szProcName, tmpProc);
                }
                else if (line.ToLower().IndexOf("create function") >= 0)
                {
                    // 解析函数
                    line = line.Substring(15).Trim();
                    string szFuncName = "";
                    index = line.IndexOf('(');
                    if (index >= 0)
                    {
                        szFuncName = line.Substring(0, index).Trim();
                    }
                    else
                    {
                        szFuncName = line;
                    }
                    int nCrc = 0;
                    FuncData tmpFunc = new FuncData();
                    
                    // 参数解析
                    while ((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if (line.ToLower().IndexOf("begin") >= 0)
                        {
                            break;
                        }

                        if(line.ToLower() == "deterministic")
                        {
                            continue;
                        }

                        ParamAttr tmpParma = new ParamAttr();
                        if (line.ToLower().IndexOf("returns") >= 0)
                        {
                            line = line.Substring(7).Trim();
                            tmpFunc.mRetVal = line;
                            nCrc += Utility.GetCRC32(tmpFunc.mRetVal);
                            continue;
                        }
                        else
                        {
                            index = line.IndexOf(' ');
                            if (index < 0)
                            {
                                if (line == ")" || line == "(")
                                {
                                    continue;
                                }
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            tmpParma.mName = line.Substring(0, index).Trim();
                            nCrc += Utility.GetCRC32(tmpParma.mName);
                            line = line.Substring(index + 1).Trim();
                            if ((index = line.IndexOf(',')) >= 0)
                            {
                                tmpParma.mType = line.Substring(0, index).Trim();
                            }
                            else
                            {
                                tmpParma.mType = line;
                            }

                            if (tmpParma.mType == "")
                            {
                                MsgOutPut(Utility.GetDictionary("str_up_table_parse_err", filename, linenum));
                                sr.Close();
                                return false;
                            }
                            nCrc += Utility.GetCRC32(tmpParma.mType);
                        }
                        tmpFunc.mParams.Add(tmpParma);
                    }

                    // 解析函数体
                    while ((line = sr.ReadLine().Trim()) != null)
                    {
                        linenum++;
                        if (line.ToLower().IndexOf("end//") >= 0 || line.ToLower().IndexOf("end;//") >= 0)
                        {
                            break;
                        }
                        tmpFunc.mLines.Add(line);
                        nCrc += Utility.GetCRC32(line);
                    }

                    tmpFunc.mCrc = nCrc;
                    funcs.Add(szFuncName, tmpFunc);
                }
            }

            return true;
        }

        // 生成升级脚本函数
        private bool GenerateUpScript(string newfile)
        {
            int index = newfile.LastIndexOf('\\');
            if(index < 0)
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_filename_err"));
                return false;
            }
            string upfile = newfile.Substring(0,index) + "\\MZXDBUpdate_0_" + fromversion.Text.Trim() + "_0_0_to_0_" + toversion.Text.Trim() + "_0_0.sql";

            FileStream fs = new FileStream(upfile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            // 生成升级脚本头
            sw.Write("/************************************************************/\n");
            sw.Write("/*数据库升级脚本\n");
            sw.Write("/*From " + fromversion.Text.Trim() + " To " + toversion.Text.Trim() + "\n");
            sw.Write("/************************************************************/\n");
            sw.Write("use " + mDbName + ";\n\n");

            sw.Write("/* 检查当前数据库版本信息 */\n");
            sw.Write("delimiter //\ndrop procedure if exists update_dbversion //\ncreate procedure update_dbversion(oldversion int ,newversion int)\nbegin\n");
            sw.Write("set @cnt = 0;\nselect count(*) into @cnt from t_general_set where skey='DBVERSION' and nVal=oldversion;\nif @cnt = 1 then\n");
            sw.Write("    update t_general_set set nVal = newversion  where sKey = \"DBVERSION\";\nelse\n    select concat('check Current DBVersion error,should be:',oldversion)  as DBVERSON;\n");
            sw.Write("end if;\nend //\ndelimiter ;\n\n");
            sw.Write("call update_dbversion(" + fromversion.Text.Trim() + ", " + toversion.Text.Trim() + ");\ndrop procedure if exists update_dbversion;\n\n");

            // 生成升级脚本内容
            // 表新建和修改相关
            foreach(KeyValuePair<String, TableAttr> st in mNewTables)
            {
                TableAttr newtable = st.Value;
                if (mOldTables.ContainsKey(st.Key) == false)     // 新加的表
                {
                    // 表头
                    sw.Write("\ndrop table if exists " + st.Key + ";\ncreate table " + st.Key + "\n(\n");
                    // 列
                    bool bFlag = false;
                    Dictionary<string, ColumnData> tc = newtable.mColumns;
                    if(tc.ContainsKey("aid"))
                    {
                        sw.Write("    aid " + tc["aid"].mType + " not null auto_increment");
                        bFlag = true;
                    }

                    foreach (KeyValuePair<string, ColumnData> sc in tc)
                    {
                        if (sc.Key == "aid")
                        {
                            continue;
                        }
                        if(bFlag == true)
                        {
                            sw.Write(",\n");
                        }
                        else
                        {
                            bFlag = true;
                        }
                        sw.Write("    " + sc.Key + ' ' + sc.Value.mType + " not null default " + sc.Value.mDefValue);
                    }
                    // 主键
                    if(newtable.mPrimaryKey.Count > 0)
                    {
                        sw.Write(",\n    primary key(");
                        for(int i = 0; i < newtable.mPrimaryKey.Count; ++i)
                        {
                            if (i > 0)
                            {
                                sw.Write(',' + newtable.mPrimaryKey[i]);
                            }
                            else
                            {
                                sw.Write(newtable.mPrimaryKey[i]);
                            }
                        }
                        sw.Write(")");
                    }
                    // 索引
                    if (newtable.mIndexs.Count > 0)
                    {
                        sw.Write(",\n");
                        bFlag = false;
                        Dictionary<string, IndexData> idata = newtable.mIndexs;
                        foreach (KeyValuePair<string, IndexData> si in idata)
                        {
                            if (bFlag)
                            {
                                sw.Write(",\n");
                            }
                            else
                            {
                                bFlag = true;
                            }

                            if (si.Value.mIsUnique)
                            {
                                sw.Write("    unique index " + si.Key + "(");
                            }
                            else
                            {
                                sw.Write("    index " + si.Key + "(");
                            }
                            for (int i = 0; i < si.Value.mCols.Count; ++i)
                            {
                                if (i > 0)
                                {
                                    sw.Write(',' + si.Value.mCols[i]);
                                }
                                else
                                {
                                    sw.Write(si.Value.mCols[i]);
                                }
                            }
                            sw.Write(')');
                        }
                    }// if (table.mIndexs.Count > 0)

                    sw.Write("\n)ENGINE = INNODB;\n");
                }// 新加表判断结束
                else if(st.Value.GetCrc() != mOldTables[st.Key].GetCrc())
                {
                    // 表有改动
                    sw.Write("\nalter table " + st.Key + "\n");

                    TableAttr oldtable = mOldTables[st.Key];
                    if (oldtable.mCrcCol != newtable.mCrcCol)   // 列有改动
                    {
                        // 先检查增加及有改变的列
                        Dictionary<string, ColumnData> newtc = newtable.mColumns;
                        Dictionary<string, ColumnData> oldtc = oldtable.mColumns;
                        bool bFlag = false;
                        foreach (KeyValuePair<string, ColumnData> sc in newtc)
                        {
                            if(oldtc.ContainsKey(sc.Key) && oldtc[sc.Key].mCrc == sc.Value.mCrc)
                            {
                                continue;
                            }

                            if (bFlag)
                            {
                                sw.Write(",\n");
                            }
                            else
                            {
                                bFlag = true;
                            }
                            if (oldtc.ContainsKey(sc.Key) == false)
                            {
                                // 新列
                                if(sc.Key == "aid")
                                {
                                    // 如果新加的列是aid，即给原来的表添加自增主键，那么表的新的主键必须是aid，否则生成报错
                                    if(!(st.Value.mPrimaryKey.Count == 1 && st.Value.mPrimaryKey[0] == "aid"))
                                    {
                                        MsgOutPut(Utility.GetDictionary("str_up_table_aid_err", st.Key));
                                        return false;
                                    }
                                    // 先删掉原来的主键
                                    sw.Write("    drop primary key,\n");
                                    sw.Write("    add column aid " + sc.Value.mType + " not null auto_increment,\n");
                                    sw.Write("    add primary key(aid)");
                                }
                                else
                                {
                                    sw.Write("    add column " + sc.Key + ' ' + sc.Value.mType + " not null default " + sc.Value.mDefValue);
                                }
                            }
                            else if(sc.Value.mType == oldtc[sc.Key].mType && sc.Value.mDefValue != oldtc[sc.Key].mDefValue)
                            {
                                // 默认值有修改
                                sw.Write("    alter column " + sc.Key + " set default " + sc.Value.mDefValue);
                            }
                            else
                            {
                                // 列定义修改
                                sw.Write("    modify column " + sc.Key + ' ' + sc.Value.mType + " not null default " + sc.Value.mDefValue);
                            }
                        }
                        //sw.Flush();

                        // 再检查被删除的列
                        foreach(KeyValuePair<string, ColumnData> sc in oldtc)
                        {
                            if (newtc.ContainsKey(sc.Key) == false)
                            {
                                // 删除列
                                if (bFlag)
                                {
                                    sw.Write(",\n");
                                }
                                else
                                {
                                    bFlag = true;
                                }

                                sw.Write("    drop column " + sc.Key);
                                bFlag = true;
                            }
                        }

                        sw.Write(";\n");
                        //sw.Flush();
                    }// 列改动判断结束

                    if (newtable.mPrimaryCrc != oldtable.mPrimaryCrc && !(newtable.mPrimaryKey.Count == 1 && newtable.mPrimaryKey[0] == "aid"))
                    {
                        // 主键有改动，并且主键不是aid，修改主键
                        sw.Write("\nalter table " + st.Key + " drop primary key;\n");
                        sw.Write("\nalter table " + st.Key + " add primary key(");
                        for (int i = 0; i < newtable.mPrimaryKey.Count; ++i)
                        {
                            if (i == 0)
                            {
                                sw.Write(newtable.mPrimaryKey[i]);
                            }
                            else
                            {
                                sw.Write(',' + newtable.mPrimaryKey[i]);
                            }
                        }
                        sw.Write(")\n");
                    }// 主键改动判断结束

                    if (newtable.mCrcIndex != oldtable.mCrcIndex)// 索引有改动，修改索引
                    {
                        // 先判断增加和修改的
                        Dictionary<string, IndexData> newIndex = newtable.mIndexs;
                        Dictionary<string, IndexData> oldIndex = oldtable.mIndexs;
                        foreach(KeyValuePair<string, IndexData> si in newIndex)
                        {
                            if (oldIndex.ContainsKey(si.Key) && oldIndex[si.Key].mCrc == si.Value.mCrc)
                            {
                                // 没有修改
                                continue;
                            }
                            if(oldIndex.ContainsKey(si.Key))
                            {
                                // 有修改，先将原来的drop掉
                                sw.Write("\ndrop index " + si.Key + " on " + st.Key + ";\n");
                            }

                            if (si.Value.mIsUnique)
                            {
                                sw.Write("\ncreate unique index " + si.Key + " on " + st.Key + "\n(\n");
                            }
                            else
                            {
                                sw.Write("\ncreate index " + si.Key + " on " + st.Key + "\n(\n");
                            }
                            for (int i = 0; i < si.Value.mCols.Count; ++i)
                            {
                                if (i == 0)
                                {
                                    sw.Write("    " + si.Value.mCols[i]);
                                }
                                else
                                {
                                    sw.Write(",\n    " + si.Value.mCols[i]);
                                }
                            }
                            sw.Write("\n);\n");
                        }

                        // 再生成删除索引的语句
                        foreach(KeyValuePair<string, IndexData> si in oldIndex)
                        {
                            if(!newIndex.ContainsKey(si.Key))
                            {
                                sw.Write("\ndrop index " + si.Key + " on " + st.Key + ";\n");
                            }
                        }

                    }// 索引改动判断结束

                }// 表有改动判断结束
            }

            // 删除表相关
            foreach(KeyValuePair<string, TableAttr> st in mOldTables)
            {
                if(!mNewTables.ContainsKey(st.Key))
                {
                    // 表已被删除
                    sw.Write("\ndrop table if exists " + st.Key + ";\n");
                }
            }

            // 存储过程相关（增加或修改）
            foreach(KeyValuePair<string, ProcData> sp in mNewProcs)
            {
                if(!mOldProcs.ContainsKey(sp.Key) || mOldProcs[sp.Key].mCrc != sp.Value.mCrc)
                {
                    sw.Write("\n\ndelimiter //\n");
                    sw.Write("drop procedure if exists " + sp.Key + "//\n");
                    if(sp.Value.mParams.Count == 0)
                    {
                        sw.Write("create procedure " + sp.Key + "()\n");
                    }
                    else
                    {
                        sw.Write("create procedure " + sp.Key + "\n(\n");
                        List<ParamAttr> lp = sp.Value.mParams;
                        for (int i = 0; i < lp.Count; ++i)
                        {
                            if(i == 0)
                            {
                                sw.Write("    " + lp[i].mName + ' ' + lp[i].mType);
                            }
                            else
                            {
                                sw.Write(",\n    " + lp[i].mName + ' ' + lp[i].mType);
                            }
                        }
                        sw.Write("\n)\n");
                    }

                    sw.Write("begin\n");
                    for(int i = 0; i < sp.Value.mLines.Count; ++i)
                    {
                        sw.Write(sp.Value.mLines[i] + '\n');
                    }

                    sw.Write("end//\n");
                    sw.Write("delimiter ;\n");
                }
            }

            // 存储过程相关（删除）
            foreach(KeyValuePair<string, ProcData> sp in mOldProcs)
            {
                if(mNewProcs.ContainsKey(sp.Key) == false)
                {
                    // 删除存储过程
                    sw.Write("\ndrop procedure if exists " + sp.Key + ";\n");
                }
            }

            // 函数相关（增加或修改）
            foreach(KeyValuePair<string, FuncData> sf in mNewFuncs)
            {
                if(!mOldFuncs.ContainsKey(sf.Key) || sf.Value.mCrc != mOldFuncs[sf.Key].mCrc)
                {
                    sw.Write("\n\ndelimiter //\n");
                    sw.Write("drop function if exists " + sf.Key + "//\n");
                    if(sf.Value.mParams.Count == 0)
                    {
                        sw.Write("create function " + sf.Key + "()\n");
                    }
                    else
                    {
                        sw.Write("create function " + sf.Key + "\n(\n");
                        List<ParamAttr> lp = sf.Value.mParams;
                        for (int i = 0; i < lp.Count; ++i)
                        {
                            sw.Flush();
                            if (i == 0)
                            {
                                sw.Write("    " + lp[i].mName + ' ' + lp[i].mType);
                            }
                            else
                            {
                                sw.Write(",\n    " + lp[i].mName + ' ' + lp[i].mType);
                            }
                        }
                        sw.Flush();
                        sw.Write("\n)\n");
                    }
                    sw.Write("returns " + sf.Value.mRetVal + "\nDETERMINISTIC\nbegin\n");

                    for (int i = 0; i < sf.Value.mLines.Count; ++i)
                    {
                        sw.Write(sf.Value.mLines[i] + '\n');
                    }

                    sw.Write("end//\n");
                    sw.Write("delimiter ;\n");
                }
            }

            // 函数删除
            foreach (KeyValuePair<string, FuncData> sf in mOldFuncs)
            {
                if(!mNewFuncs.ContainsKey(sf.Key))
                {
                    sw.Write("\ndrop function if exists " + sf.Key + ";\n");
                }
            }

            sw.Flush();
            sw.Close();
            fs.Close();

            return true;
        }
        
        private void geupdate_Click(object sender, RoutedEventArgs e)
        {
            string oldfile = oldpath.Text;
            if (oldfile == "")
            {
                Utility.MessageBoxNotice("str_old_script_need");
                return;
            }

            string newfile = newpath.Text;
            if (newfile == "")
            {
                Utility.MessageBoxNotice("str_new_script_need");
                return;
            }

            if (fromversion.Text == "")
            {
                Utility.MessageBoxNotice("str_from_version_need");
                return;
            }

            if (toversion.Text == "")
            {
                Utility.MessageBoxNotice("str_to_version_need");
                return;
            }

            resultmsg.Text = "";

            // 开始读取并解析旧的建库脚本
            if (read_init_script(oldfile, mOldTables, mOldProcs, mOldFuncs) == false)
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_old_err"));
                return;
            }
            MsgOutPut(Utility.GetDictionary("str_up_table_old_over"));

            // 开始读取并解析新的建库脚本
            if (read_init_script(newfile, mNewTables, mNewProcs, mNewFuncs) == false)
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_new_err"));
                return;
            }
            MsgOutPut(Utility.GetDictionary("str_up_table_new_over"));

            if(mDbName == "")
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_dbname_err"));
                return;
            }

            // 开始生成新的升级脚本
            int index = newfile.LastIndexOf('\\');
            if (index < 0)
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_filename_err"));
                return ;
            }
            string upfile = newfile.Substring(0, index) + "\\MZXDBUpdate_0_" + fromversion.Text.Trim() + "_0_0_to_0_" + toversion.Text.Trim() + "_0_0.sql";
            if(!GenerateUpScript(upfile))
            {
                MsgOutPut(Utility.GetDictionary("str_up_table_file_generate_err"));
                return;
            }
            MsgOutPut(Utility.GetDictionary("str_up_table_file_generate_success", upfile));

        }

        private void GenerateCheck(string szCheckFile, string szUpFile)
        {
            // 解析完毕，开始生成检查脚本
            FileStream fs = new FileStream(szCheckFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            // 生成检查脚本头
            int index = szUpFile.LastIndexOf('\\');
            if(index >= 0)
            {
                sw.Write("/************************************************************/\n");
                sw.Write("/* 数据库升级后检查数据库脚本\n");
                sw.Write("/* 版本升级脚本： " + szUpFile.Substring(index+1) + "\n");
                sw.Write("/************************************************************/\n");
            }
            sw.Write("use " + mDbName + ";\n\n");

            // 生成检查脚本内容
            int check_cnt = 0;

            sw.Write("DELIMITER //\ndrop procedure if exists check_result //\ncreate procedure check_result ()\nbegin\nset @check_cnt = 0;\n\nset @dbnames = database();\n");

            string errMsg = "";
            if (mTables.Count > 0)
            {
                sw.Write("/* 检查本次修改的表格 */\n\nset @cnt = 0;\n");
                sw.Write("select count(*) into @cnt from information_schema.tables where table_schema=@dbnames and table_name in (");
                bool bInit = false;
                foreach (KeyValuePair<string, int> tc in mTables)
                {
                    if (bInit)
                    {
                        sw.Write(",\'" + tc.Key + '\'');
                        errMsg += ("," + tc.Key);
                    }
                    else
                    {
                        sw.Write('\'' + tc.Key + '\'');
                        errMsg += tc.Key;
                        bInit = true;
                    }
                }
                sw.Write(");\n");
                sw.Write("if @cnt = " + mAddColumns.Count().ToString() + " then\n");
                sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check table error:" + errMsg);
                sw.Write("\',@cnt);\nend if;\n\n");
                check_cnt++;
            }

            int nCount = mProcedures.Count();
            if(nCount > 0)
            {
                sw.Write("/* 检查存储过程 */\n");
                sw.Write("set @cnt = 0;\n");
                sw.Write("select count(*) into @cnt from information_schema.ROUTINES where ROUTINE_SCHEMA=@dbnames and ROUTINE_TYPE='PROCEDURE' and ROUTINE_NAME in (");

                errMsg = "";
                for (int i = 0; i < nCount; ++i)
                {
                    if (i + 1 < nCount)
                    {
                        sw.Write('\'' + mProcedures[i] + "\',");
                        errMsg += (mProcedures[i] + ',');
                    }
                    else
                    {
                        sw.Write('\'' + mProcedures[i] + "\');\n");
                        errMsg += (mProcedures[i] + "\',@cnt);\nend if;\n\n");
                    }
                }
                sw.Write("if @cnt = " + nCount.ToString() + " then\n");
                sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check procedure error:" + errMsg);
                check_cnt++;
            }

            if(mAddColumns.Count > 0)
            {
                sw.Write("/* 检查本次修改表格的正确性 */\n");
                foreach (KeyValuePair<string, List<string>> tc in mAddColumns)
                {
                    sw.Write("set @cnt = 0;\n");
                    sw.Write("select count(*) into @cnt from information_schema.columns where table_schema=@dbnames and table_name='" + tc.Key + '\'' + " and column_name in (");
                    List<string> lCols = tc.Value;
                    nCount = lCols.Count();
                    errMsg = "";
                    for (int i = 0; i < nCount; ++i)
                    {
                        if (i + 1 < nCount)
                        {
                            sw.Write('\'' + lCols[i] + "\',");
                            errMsg += (lCols[i] + ',');
                        }
                        else
                        {
                            sw.Write('\'' + lCols[i] + "\');\n");
                            errMsg += (lCols[i] + "\',@cnt);\nend if;\n\n");
                        }
                    }

                    sw.Write("if @cnt = " + nCount.ToString() + " then\n");
                    sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check Table add or change column error:" + errMsg);
                    check_cnt++;
                }
            }

            if(mDelColumns.Count > 0)
            {
                sw.Write("/* 检查本次表格删除的列是否仍然存在 */\n");
                foreach (KeyValuePair<string, List<string>> tc in mDelColumns)
                {
                    sw.Write("set @cnt = 0;\n");
                    sw.Write("select count(*) into @cnt from information_schema.columns where table_schema=@dbnames and table_name='" + tc.Key + '\'' + " and column_name in (");
                    List<string> lCols = tc.Value;
                    nCount = lCols.Count();
                    errMsg = "";
                    for (int i = 0; i < nCount; ++i)
                    {
                        if (i + 1 < nCount)
                        {
                            sw.Write('\'' + lCols[i] + "\',");
                            errMsg += (lCols[i] + ',');
                        }
                        else
                        {
                            sw.Write('\'' + lCols[i] + "\');\n");
                            errMsg += (lCols[i] + "\',@cnt);\nend if;\n\n");
                        }
                    }
                    sw.Write("if @cnt = 0 then\n");
                    sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check Table del column error:" + errMsg);
                    check_cnt++;
                }
            }

            if(mCheckIndex.Count > 0)
            {
                sw.Write("/* 检查新加索引是否存在 */\n");
                foreach (KeyValuePair<string, IndexCheckData> si in mCheckIndex)
                {
                    sw.Write("set @cnt = 0;\n");
                    string szKeyName = si.Key;
                    if (si.Key == ("PRIMARY_" + si.Value.mTable))
                    {
                        szKeyName = "PRIMARY";
                    }

                    sw.Write("select count(*) into @cnt from information_schema.statistics where INDEX_SCHEMA=@dbnames and TABLE_NAME='" + si.Value.mTable + "' and INDEX_NAME='" + szKeyName + "' and column_name in (");
                    List<string> lCols = si.Value.mCols;
                    nCount = lCols.Count();
                    errMsg = "";
                    for (int i = 0; i < nCount; ++i)
                    {
                        if (i + 1 < nCount)
                        {
                            sw.Write('\'' + lCols[i] + "\',");
                            errMsg += (lCols[i] + ',');
                        }
                        else
                        {
                            sw.Write('\'' + lCols[i] + "\');\n");
                            errMsg += (lCols[i] + "\',@cnt);\nend if;\n\n");
                        }
                    }
                    sw.Write("if @cnt = " + nCount.ToString() + " then\n");
                    sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check table " + si.Value.mTable + " index " + szKeyName + " add error:" + errMsg);
                    sw.Flush();
                    check_cnt++;
                }
            }

            if(mDelIndex.Count > 0)
            {
                sw.Write("/* 检查删除索引是否仍然存在 */\n");
                foreach(KeyValuePair<string, string> ss in mDelIndex)
                {
                    string szKeyName = ss.Key;
                    if(ss.Key == ("PRIMARY_" + ss.Value))
                    {
                        szKeyName = "PRIMARY";
                    }
                    if(!mCheckIndex.ContainsKey(ss.Key))
                    {
                        sw.Write("set @cnt = 0;\n");
                        sw.Write("select count(*) from information_schema.statistics where INDEX_SCHEMA=@dbnames and TABLE_NAME='" + ss.Value + "\' and INDEX_NAME='" + szKeyName + "';\n");
                        sw.Write("if @cnt = 0 then\n");
                        sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check table " + ss.Value + " index del error: " + szKeyName + " already exists',@cnt);\nend if;\n\n");
                        check_cnt++;
                    }
                }
            }

            nCount = mFuncs.Count();
            if (nCount > 0)
            {
                sw.Write("/* 检查新增函数是否存在 */\n");
                sw.Write("set @cnt = 0;\n");
                sw.Write("select count(*) into @cnt from information_schema.ROUTINES where ROUTINE_SCHEMA=@dbnames and ROUTINE_TYPE='FUNCTION' and ROUTINE_NAME in (");

                errMsg = "";
                for (int i = 0; i < nCount; ++i)
                {
                    if (i + 1 < nCount)
                    {
                        sw.Write('\'' + mFuncs[i] + "\',");
                        errMsg += (mFuncs[i] + ',');
                    }
                    else
                    {
                        sw.Write('\'' + mFuncs[i] + "\');\n");
                        errMsg += (mFuncs[i] + "\',@cnt);\nend if;\n\n");
                    }
                }
                sw.Write("if @cnt = " + nCount.ToString() + " then\n");
                sw.Write("    set @check_cnt = @check_cnt+1;\nelse\n    select concat('check function error:" + errMsg);
                check_cnt++;
            }

            sw.Write("/* 检查当前数据库版本信息 */\n");
            sw.Write("set @cnt = 0;\nselect count(*) into @cnt from t_general_set where skey='DBVERSION' and nVal=" + mVersion.ToString() + ";\n");
            sw.Write("if @cnt = 1 then\n    set @check_cnt = @check_cnt+1;\nelse\n    select 'check DBVersion error:" + mVersion.ToString() + "\' as DBVERSON;\nend if;\n\n\n");
            check_cnt++;

            sw.Write("if @check_cnt = " + check_cnt.ToString() + " then\n");
            sw.Write("    select 'check ok' as SUCCESSFUL;\nelse\n");
            sw.Write("    select concat('check error:',@check_cnt,'!=" + check_cnt.ToString() + "') as ERRORREPORT;\nend if;\nend//\n");
            sw.Write("DELIMITER ;\n\n");

            sw.Write("call check_result();\n");
            sw.Write("drop procedure if exists check_result;\n");
            sw.Write("\n");

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void gecheck_Click(object sender, RoutedEventArgs e)
        {
            string upfile = uppath.Text;
            if (upfile == "")
            {
                Utility.MessageBoxNotice("str_up_script_need");
                return;
            }

            int index_ex = -1;
            int index = upfile.LastIndexOf('.');
            string checkfile = upfile.Substring(0, index) + "_check.sql";
            gecheck.IsEnabled = false;

            // 开始读取升级脚本文件
            StreamReader sr = new StreamReader(upfile, Encoding.UTF8);
            string line;
            int linenum = 0;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine().Trim();
                linenum++;
                if (line == "" || line[0] == '#' || (line[0] == '/' && line[1] == '*'))
                {
                    continue;           // 注释略过
                }

                if ((index = line.ToLower().IndexOf("use ")) >= 0)
                {
                    // 记录数据库的名字
                    line = line.Substring(4).Trim();
                    index = line.LastIndexOf(';');
                    mDbName = line.Substring(0, index).Trim();
                }
                else if( (line.ToLower().IndexOf("create procedure")) >= 0)
                {
                    index = line.LastIndexOf('(');
                    if(index >= 0)
                    {
                        line = line.Substring(0, index).Trim();
                    }
                    index = line.LastIndexOf(' ');
                    string procname = line.Substring(index).Trim();
                    if (procname == "update_dbversion")
                    {
                        continue;       // 不是增加的存储过程
                    }
                    mProcedures.Add(procname);
                }
                else if (line.ToLower().IndexOf("call update_dbversion") >= 0 )
                {
                    // 版本号记录
                    index = line.IndexOf(",");
                    if (index < 0)
                    {
                        Utility.MessageBoxNotice("str_check_version_err");
                        sr.Close();
                        return;
                    }
                    string szStr = line.Substring(index+1).Trim();
                    index = szStr.IndexOf(")");
                    if (index < 0)
                    {
                        Utility.MessageBoxNotice("str_check_version_err");
                        sr.Close();
                        return;
                    }
                    mVersion = int.Parse(szStr.Substring(0, index));
                }
                else if((line.ToLower().IndexOf("create function")) >= 0)
                {
                    index = line.LastIndexOf('(');
                    if (index >= 0)
                    {
                        line = line.Substring(0, index).Trim();
                    }
                    index = line.LastIndexOf(' ');
                    string funcname = line.Substring(index).Trim();
                    mFuncs.Add(funcname);
                }
                else if (line.ToLower().IndexOf("create table") >= 0)     // 这里的解析需要遵循sql脚本的编码规范
                {
                    // 表的改动
                    index = line.LastIndexOf(' ');
                    string tname = line.Substring(index).Trim();

                    List<string> lCols = new List<string>();
                    // 找到表的第一列所在的行
                    while ((line = sr.ReadLine().Trim().ToLower()) != null)
                    {
                        linenum++;
                        if (line.IndexOf(',') >= 0 || line.IndexOf(';') >= 0)
                        {
                            break;
                        }
                    }

                    // 开始解析列名
                    while(line.IndexOf(';') < 0)
                    {  
                        if (line.ToLower().IndexOf("not null") >= 0)
                        {
                            index = line.IndexOf(' ');
                            string szCol = line.Substring(0, index).Trim();
                            lCols.Add(szCol);
                        }
                        line = sr.ReadLine().Trim();
                        if(line == null)
                        {
                            string szResult = Utility.GetDictionary("str_check_parse_err", linenum);
                            MessageBox.Show(szResult);
                            sr.Close();
                            return;
                        }
                        linenum++;
                    }

                    if(lCols.Count() <= 0)
                    {
                        string szResult = Utility.GetDictionary("str_check_table_err", tname);
                        MessageBox.Show(szResult);
                        sr.Close();
                        return;
                    }
                    mAddColumns.Add(tname, lCols);
                    mTables.Add(tname, 1);
                }
                else if (line.ToLower().IndexOf("alter table") >= 0)      // 这里的解析需要遵循sql脚本的编码规范
                {
                    string tname = line.Substring(11).Trim();
                    List<string> lCols = new List<string>();
                    List<string> delCols = new List<string>();

                    do
                    {
                        line = sr.ReadLine().Trim();
                        if (line == null)
                        {
                            MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                            sr.Close();
                            return;
                        }
                        linenum++;

                        if (line.ToLower().IndexOf("add column") >= 0)
                        {
                            // 新增列
                            line = line.Substring(10).Trim();
                            index = line.IndexOf(' ');
                            if(index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                                sr.Close();
                                return;
                            }
                            string szCol = line.Substring(0, index).Trim();
                            lCols.Add(szCol);
                        }
                        else if(line.ToLower().IndexOf("drop column") >= 0)
                        {
                            // 删除列
                            line = line.Substring(11);
                            if((index = line.IndexOf(',')) >= 0 || (index = line.IndexOf(';')) >= 0)
                            {
                                string szCol = line.Substring(0, index);
                                delCols.Add(szCol);
                            }
                            else
                            {
                                MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                                sr.Close();
                                return;
                            }
                        }
                        else if(line.ToLower().IndexOf("add primary key") >= 0)
                        {
                            // 新增主键索引
                            string szCol = "";
                            IndexCheckData icd = new IndexCheckData();
                            icd.mTable = tname;
                            index = line.IndexOf('(');
                            if(index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                                sr.Close();
                                return;
                            }
                            line = line.Substring(index + 1);
                            while ((index = line.IndexOf(',')) >= 0 && line[index - 1] != ')')
                            {
                                szCol = line.Substring(0, index).Trim();
                                icd.mCols.Add(szCol);
                                line = line.Substring(index + 1).Trim();
                            }
                            index = line.IndexOf(')');
                            if (index < 0)
                            {
                                MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                                sr.Close();
                                return;
                            }
                            szCol = line.Substring(0, index);
                            icd.mCols.Add(szCol);
                            mCheckIndex.Add("PRIMARY_" + tname, icd);
                        }
                        else if(line.ToLower().IndexOf("drop primary key") >= 0)
                        {
                            mDelIndex.Add("PRIMARY_" + tname, tname);
                        }
                    } while (line.IndexOf(';') < 0);

                    if (lCols.Count() > 0)
                    {
                        mAddColumns.Add(tname, lCols);
                    }

                    if(delCols.Count() > 0)
                    {
                        mDelColumns.Add(tname, delCols);
                    }
                    mTables.Add(tname, 1);
                }
                else if((index=line.ToLower().IndexOf("create unique index")) >= 0 || (index_ex=line.ToLower().IndexOf("create index")) >= 0)
                {
                    if(index >= 0)
                    {
                        line = line.Substring(19).Trim();
                    }
                    else
                    {
                        line = line.Substring(12).Trim();
                    }
                    index = line.IndexOf("on ");
                    if (index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                        sr.Close();
                        return;
                    }
                    IndexCheckData icd = new IndexCheckData();
                    string iname = line.Substring(0, index).Trim();
                    index = line.LastIndexOf(' ');
                    icd.mTable = line.Substring(index).Trim();
                    do
                    {
                        line = sr.ReadLine().Trim();
                        if(line == null)
                        {
                            MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                            sr.Close();
                            return;
                        }
                        linenum++;
                        if(line == "(" || line == ");")
                        {
                            continue;
                        }
                        if((index = line.IndexOf(',')) >= 0)
                        {
                            string szCol = line.Substring(0, index).Trim();
                            icd.mCols.Add(szCol);
                        }
                        else
                        {
                            icd.mCols.Add(line);
                        }
                    } while (line.IndexOf(';') < 0);
                    mCheckIndex.Add(iname, icd);
                }
                else if(line.ToLower().IndexOf("drop index") >= 0)
                {
                    line = line.Substring(10).Trim();
                    index = line.IndexOf("on ");
                    if(index < 0 || index <= 10)
                    {
                        MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                        sr.Close();
                        return;
                    }
                    string iname = line.Substring(0, index).Trim();
                    line = line.Substring(index + 3).Trim();
                    index = line.IndexOf(';');
                    if(index < 0)
                    {
                        MsgOutPut(Utility.GetDictionary("str_check_parse_err", linenum));
                        sr.Close();
                        return;
                    }
                    string tname = line.Substring(0, index).Trim();
                    mDelIndex.Add(iname, tname);
                }
            }
            sr.Close();

            GenerateCheck(checkfile, upfile);

            string szMsg = Utility.GetDictionary("str_check_table_success", checkfile);
            resultmsg.Text = szMsg;

            gecheck.IsEnabled = true;
        }

    }
}
