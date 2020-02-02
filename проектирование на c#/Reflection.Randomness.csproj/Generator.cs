using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Reflection.Randomness
{
    public class FromDistributionAttribute : Attribute
    {
        public IContinousDistribution GenerateDistribution { get; set; } = new NormalDistribution(-1, 2);
        public Type TypeDistribution { get; set; } = typeof(NormalDistribution);
        public static bool For;

        public static IContinousDistribution CreateGenerateDistribution(Type type = null, params int[] parameters)
        {
            if (type == typeof(ExponentialDistribution))
            {
                if (parameters.Length > 1) throw new ArgumentException($"Для типа {type} требуется 1 параметр");
                else return (parameters.Length == 0) ? new ExponentialDistribution(4) :
                new ExponentialDistribution(parameters[0]);
            }
            else if (type == typeof(NormalDistribution))
            {
                if ((parameters.Length == 1) || (parameters.Length > 2))
                    throw new ArgumentException($"Для типа {type} требуется 2 параметра");
                else return (parameters.Length == 0) ? new NormalDistribution() :
                new NormalDistribution(parameters[0], parameters[1]);
            }
            return null;
        }

        public FromDistributionAttribute(Type type = null, params int[] parameters)
        {
            TypeDistribution = (type == null) ? typeof(NormalDistribution) : type;
            GenerateDistribution = CreateGenerateDistribution(type, parameters);
        }
    }

    public class Generator<T> where T : new()
    {
        public T Generate(Random r)
        {
            var genrat = new AfterGenerate<T>();
            return genrat.Generate(r);
        }

        public delegate dynamic LParam(T t);
        public AfterFor<T> For(Expression<Func<T, double>> p)
        {
            var gen = new AfterGenerate<T>();
            return gen.For(p);
        }
    }

    public class AfterGenerate<T> where T : new()
    {
        public T TestingClass;
        public Dictionary<string, IContinousDistribution> generators;

        public AfterGenerate()
        { NewTestingClass(); }

        public void NewTestingClass()
        {
            TestingClass = new T();
            IContinousDistribution defaultEDistribution = new NormalDistribution();
            generators = new Dictionary<string, IContinousDistribution>();

            var numbers = TestingClass.GetType().GetProperties();
            var attr = numbers.Select((z) => new { val = z.GetCustomAttributes(typeof(FromDistributionAttribute), false), number = z })
            .Where(z => z.val.Length > 0);
            try { attr.Count(); }
            catch (ArgumentException e) { throw new ArgumentException(e.Message); }

            foreach (var atr in attr)
            {
                IContinousDistribution DoubleGenerator = ((FromDistributionAttribute)(atr.val.GetValue(0))).GenerateDistribution;
                generators.Add(atr.number.Name, DoubleGenerator);
            }
        }

        public T Generate(Random r)
        {
            var properties = TestingClass.GetType().GetProperties(BindingFlags.Instance |
            BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var prop in properties)
            {
                if (null != prop && prop.CanWrite)
                {
                    if (generators.ContainsKey(prop.Name))
                        prop.SetValue(TestingClass, generators[prop.Name].Generate(r), null);
                }
            }

            return TestingClass;
        }

        private static string GetName<T1, TResult>(Expression<Func<T1, TResult>> member)
        {
            MemberExpression memberExpression = member.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException();
            }
            return memberExpression.Member.Name;
        }

        public AfterFor<T> For(Expression<Func<T, double>> p)
        {
            var SelectField = GetName<T, double>(p);
            if (SelectField == null || (SelectField.GetType() != typeof(string))) throw new ArgumentException();

            if (generators.Count() == 0 || !generators.ContainsKey(SelectField))
            {
                var numbers = TestingClass.GetType().GetProperties();
                bool findField = false;
                foreach (var n in numbers)
                    if (n.Name.Equals(SelectField))
                    {
                        generators.Add(SelectField, null);
                        findField = true;
                    }
                if (!findField) throw new ArgumentException();
            }
            return new AfterFor<T>(this, TestingClass, SelectField.ToString());
        }

        public AfterGenerate<T> Set(string SelectField, IContinousDistribution d)
        {
            if (SelectField != null)
            {
                generators.Remove(SelectField);
                generators.Add(SelectField, d);
            }
            return this;
        }
    }


    public class AfterFor<T> where T : new()
    {
        T classT;
        string SelectField;
        AfterGenerate<T> Parent;

        public AfterFor(AfterGenerate<T> parent, T classT, string selectField)
        {
            this.classT = classT; Parent = parent;
            SelectField = selectField;
        }

        public delegate dynamic LParam(dynamic t);
        public AfterGenerate<T> Set(IContinousDistribution d)
        {
            return Parent.Set(SelectField, d);
        }
    }
}
