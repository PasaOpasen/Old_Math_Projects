using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
   public  class Traversal<TTree, TValue>
    {
        public static IEnumerable<TValue> GetValues(TTree tree,Func<TTree, Tuple< TValue,List<TTree>>> func)
        {
            List<TValue> res = new List<TValue>();

            void AddVal(TTree t)
            {
                var tmp = func(t);
                if(tmp.Item1!=null)
                res.Add(tmp.Item1);
                if (tmp.Item2 != null)
                    foreach (var some in tmp.Item2)
                        if (some != null)
                            AddVal(some);
            }
            AddVal(tree);
            return res;
        }
    }

    public static class Traversal
    {
        public static IEnumerable<int> GetBinaryTreeValues(BinaryTree<int> tree)
        {
            Func<BinaryTree<int>,Tuple<int, List<BinaryTree<int>>>> func = (BinaryTree<int> t) =>new Tuple<int, List<BinaryTree<int>>>(t.Value, new BinaryTree<int> []{ t.Left,t.Right }.ToList());
            return Traversal<BinaryTree<int>, int>.GetValues(tree, func);
        }
        public static IEnumerable<Job> GetEndJobs(Job tree)
        {
            Func<Job, Tuple<Job, List<Job>>> func = (Job t) => new Tuple<Job, List<Job>>((t.Subjobs==null|| t.Subjobs.Count==0)?t:null, t.Subjobs);
            return Traversal<Job, Job>.GetValues(tree, func);
        }
        public static IEnumerable<Product> GetProducts(ProductCategory tree)
        {
            Func< ProductCategory, Tuple<List<Product>, List<ProductCategory>>> func = (ProductCategory t) =>new Tuple<List<Product>, List<ProductCategory>>(t.Products,t.Categories);
            var b = Traversal<ProductCategory, List<Product>>.GetValues(tree, func).ToArray();
            List<Product> l = new List<Product>();
            foreach (var p in b)
                l.AddRange(p);
            return l;
        }
    }
}
