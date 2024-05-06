using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{





    /// <summary>
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BdNode<T>
    {
        public T Data { set; get; }
        public BdNode<T> Next { set; get; }
        public BdNode<T> Prev { set; get; }
        public BdNode(T val, BdNode<T> prev, BdNode<T> next)
        {
            this.Data = val;
            this.Prev = prev;
            this.Next = next;

        }
    }

    public class DoubleLink<T>
    {


        //表头
        private readonly BdNode<T> _linkHead;
        //节点个数
        private int _size;

        public DoubleLink()
        {
            _linkHead = new BdNode<T>(default(T), null, null);//双向链表 表头为空
            _linkHead.Prev = _linkHead;
            _linkHead.Next = _linkHead;
            _size = 0;
        }

        //获取节点数
        public int GetSize() => _size;

        //链表是否为空
        public bool IsEmpty() => (_size == 0);

        //通过索引查找
        private BdNode<T> GetNode(int index)
        {
            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException("索引溢出或者链表为空");
            if (index < _size / 2)//正向查找
            {
                BdNode<T> node = _linkHead.Next;
                for (int i = 0; i < index; i++)
                    node = node.Next;
                return node;
            }
            //反向查找
            BdNode<T> rnode = _linkHead.Prev;
            int rindex = _size - index - 1;
            for (int i = 0; i < rindex; i++)
                rnode = rnode.Prev;
            return rnode;
        }
        public T Get(int index) => GetNode(index).Data;
        public T GetFirst() => GetNode(0).Data;
        public T GetLast() => GetNode(_size - 1).Data;
        // 将节点插入到第index位置之前
        public void Insert(int index, T t)
        {
            if (_size < 1 || index >= _size)
                throw new Exception("没有可插入的点或者索引溢出了");
            if (index == 0)
                Append(_size, t);
            else
            {
                BdNode<T> inode = GetNode(index);
                BdNode<T> tnode = new BdNode<T>(t, inode.Prev, inode);
                inode.Prev.Next = tnode;
                inode.Prev = tnode;
                _size++;
            }
        }
        //追加到index位置之后
        public void Append(int index, T t)
        {
            BdNode<T> inode;
            if (index == 0)
                inode = _linkHead;
            else
            {
                index = index - 1;
                if (index < 0)
                    throw new IndexOutOfRangeException("位置不存在");
                inode = GetNode(index);
            }
            BdNode<T> tnode = new BdNode<T>(t, inode, inode.Next);
            inode.Next.Prev = tnode;
            inode.Next = tnode;
            _size++;
        }
        //删除元素
        public void Del(int index)
        {
            BdNode<T> inode = GetNode(index);
            inode.Prev.Next = inode.Next;
            inode.Next.Prev = inode.Prev;
            _size--;
        }
        public void DelFirst() => Del(0);
        public void DelLast() => Del(_size - 1);
        public void ShowAll()
        {
            Console.WriteLine("******************* 链表数据如下 *******************");
            for (int i = 0; i < _size; i++)
                Console.WriteLine("(" + i + ")=" + Get(i));
            Console.WriteLine("******************* 链表数据展示完毕 *******************\n");
        }
    }


}
