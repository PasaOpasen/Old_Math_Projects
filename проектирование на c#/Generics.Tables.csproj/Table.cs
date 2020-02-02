using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<T1, T2, TR>
    {
        public List<T1> Rows;
        public List<T2> Columns;
        Dictionary<Tuple<int, int>, TR> D3;
        public ClassOpen Open;
        public ClassExist Existed;
        public Table()
        {
            Rows = new List<T1>();
            Columns = new List<T2>();
            D3 = new Dictionary<Tuple<int, int>, TR>();
            Open = new ClassOpen(this);
            Existed = new ClassExist(this);
        }
        public void AddRow(T1 val) {
            if(!Rows.Contains(val))
            Rows.Add(val);
        }
        public void AddColumn(T2 val)
        {
            if (!Columns.Contains(val))
                Columns.Add(val);
        }
        public void Set(T1 row, T2 col, TR value)
        {
            int t1, t2;
            if (Rows.Contains(row))
                t1 = Rows.IndexOf(row);
            else
            {
                AddRow(row);
                t1 = Rows.Count - 1;
            }
            if (Columns.Contains(col))
                t2 = Columns.IndexOf(col);
            else
            {
                AddColumn(col);
                t2 = Columns.Count - 1;
            }
            D3.Add(new Tuple<int, int>(t1, t2), value);
        }
        public TR GetIfExist(T1 row, T2 col)
        {
            var key = new Tuple<int, int>(Rows.IndexOf(row), Columns.IndexOf(col));
            if (D3.ContainsKey(key))
                return D3[key];
            D3.Add(key, default(TR));
            return default(TR);
        }

        public class ClassOpen
        {
            Table<T1, T2, TR> Table;
            public ClassOpen(Table<T1, T2, TR> Table)
            {
                this.Table = Table;
            }

            public TR this[T1 row, T2 col]
            {
                get
                {
                    if (Table.Rows.Contains(row) && Table.Columns.Contains(col))
                    {
                        return Table.GetIfExist(row, col);
                    }

                    //Table.Set(row, col, default(TR));
                    return default(TR);
                }
                set
                {
                    Table.Set(row, col, value);
                }
            }
        }
        public class ClassExist
        {
            Table<T1, T2, TR> Table;
            public ClassExist(Table<T1, T2, TR> Table)
            {
                this.Table = Table;
            }

            public TR this[T1 row, T2 col]
            {
                get
                {
                    if (!(Table.Rows.Contains(row) && Table.Columns.Contains(col)))
                        throw new ArgumentException();

                    return Table.GetIfExist(row, col);
                }
                set
                {
                    if (!(Table.Rows.Contains(row) && Table.Columns.Contains(col)))
                        throw new ArgumentException();
                    Table.Set(row, col, value);
                }
            }
        }
    }
}
