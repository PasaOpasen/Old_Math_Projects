using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{

    public class BinaryTree<T> : IEnumerable
    where T : IComparable
    {
        public T Value { get; set; }
        public BinaryTree<T> Left, Right;
        private bool IsEmpty = true;

        public BinaryTree() { }
        public BinaryTree(T val)
        {
            IsEmpty = false;
            Value = val;
        }
        public BinaryTree(T[] val)
        {
           // IsEmpty = false;
            for (int i = 0; i < val.Length; i++)
                Add(val[i]);
        }
        public void Add(T value)
        {
            // Случай 1: Если дерево пустое, просто создаем корневой узел.
            if (IsEmpty)
            {
                IsEmpty = false;
                Value = value;
            }
            // Случай 2: Дерево не пустое => 
            // ищем правильное место для вставки.
            else
            {
                AddTo(this, value);
            }
        }

        // Рекурсивная ставка.
        private void AddTo(BinaryTree<T> node, T value)
        {
            // Случай 1: Вставляемое значение меньше значения узла
            if (value.CompareTo(node.Value) <= 0)
            {
                // Если нет левого поддерева, добавляем значение в левого ребенка,
                if (node.Left==null)
                {               
                    node.Left = new BinaryTree<T>(value);
                }
                else
                {
                    // в противном случае повторяем для левого поддерева.
                    AddTo(node.Left, value);
                }
            }
            // Случай 2: Вставляемое значение больше или равно значению узла.
            else
            {
                // Если нет правого поддерева, добавляем значение в правого ребенка,
                if (node.Right == null)
                {
                    node.Right = new BinaryTree<T>(value);
                }
                else
                {
                    // в противном случае повторяем для правого поддерева.
                    AddTo(node.Right, value);
                }
            }
        }


        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, this);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTree<T> node)
        {
            if (node != null)
            {
                action(node.Value);
                PreOrderTraversal(action, node.Left);
                PreOrderTraversal(action, node.Right);
            }
        }
        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, this);
        }

        private void PostOrderTraversal(Action<T> action, BinaryTree<T> node)
        {
            if (node != null)
            {
                PostOrderTraversal(action, node.Left);
                PostOrderTraversal(action, node.Right);
                action(node.Value);
            }
        }

        public void InOrderTraversal(Action<T> action)
        {
            InOrderTraversal(action, this);
        }

        private void InOrderTraversal(Action<T> action, BinaryTree<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);

                action(node.Value);

                InOrderTraversal(action, node.Right);
            }
        }

        public IEnumerator InOrderTraversal()
        {
            if (IsEmpty)
                yield break;
            // Это нерекурсивный алгоритм.
            // Он использует стек для того, чтобы избежать рекурсии.
            if (Value != null)
            {
                // Стек для сохранения пропущенных узлов.
                Stack stack = new Stack();

                BinaryTree<T> current = this;

                // Когда мы избавляемся от рекурсии, нам необходимо
                // запоминать, в какую стороны мы должны двигаться.
                bool goLeftNext = true;

                // Кладем в стек корень.
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // Если мы идем налево...
                    if (goLeftNext)
                    {
                        // Кладем все, кроме самого левого узла на стек.
                        // Крайний левый узел мы вернем с помощю yield.
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // Префиксный порядок: left->yield->right.
                    yield return current.Value;

                    // Если мы можем пойти направо, идем.
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // После того, как мы пошли направо один раз,
                        // мы должным снова пойти налево.
                        goLeftNext = true;
                    }
                    else
                    {
                        // Если мы не можем пойти направо, мы должны достать родительский узел
                        // со стека, обработать его и идти в его правого ребенка.
                        current = (BinaryTree<T>)stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {

            return InOrderTraversal();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public T First() => Value;
    }

    public class BinaryTree
    {
        public static BinaryTree<T> Create<T> (params T[] arr)where T:IComparable => new BinaryTree<T>(arr);
    }

}