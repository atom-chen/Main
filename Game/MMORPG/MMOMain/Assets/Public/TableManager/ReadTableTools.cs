using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Public.TableManager
{
    /// <summary>
    /// 表格文件加载类
    /// </summary>
    class ReadTableTools
    {
        private List<string>[] m_Datas;//所有字段组成的二维数组：每一维表示一个元组
        int row = 0;//当前该行的第几个数据
        int line = 0;//当前读到第几行
        private const string TablePathRoot = "F:\\Work\\Game\\MMORPG\\MMOMain\\Assets\\Public\\Table\\";

        /// <summary>
        /// 有几个元组
        /// </summary>
        public int TupleCount
        {
            get
            {
                return m_Datas != null ? m_Datas.Length : 0;
            }
        }

        /// <summary>
        /// 每个元组有几列属性
        /// </summary>
        public int AttributeCount
        {
            get
            {
                return m_Datas[0] != null ? m_Datas[0].Count : 0;
            }
        }

        public ReadTableTools(string tableName)
        {
            string[] contents = File.ReadAllLines(TablePathRoot + tableName, Encoding.UTF8);
            if (contents == null)
            {
                throw new Exception(tableName + "表格为空");
            }
            m_Datas = new List<string>[contents.Length - 1];
            for (int i = 1; i < contents.Length; i++)
            {
                string[] data = contents[i].Split(' ', '\t');
                m_Datas[i - 1] = new List<string>();
                for (int j = 0; j < data.Length; j++)
                {
                    m_Datas[i - 1].Add(data[j]);
                }
            }

        }

        //获取当前游标所指位置的数据
        public string GetData()
        {
            if (line >= m_Datas.Length || row >= m_Datas[line].Count)
            {
                return "";
            }
            return m_Datas[line][row];
        }

        //获取当前元组的下一个数据
        public string GetNext()
        {
            if (line >= m_Datas.Length || row >= m_Datas[line].Count)
            {
                return "";
            }
            row++;
            return m_Datas[line][row];
        }

        //下一行
        public void LineDown()
        {
            line++;
            row = 0;

        }

        /// <summary>
        /// 当前是否最后一行
        /// </summary>
        public bool IsTupleEnd()
        {
            return line >= m_Datas.Length;
        }
    }
}
