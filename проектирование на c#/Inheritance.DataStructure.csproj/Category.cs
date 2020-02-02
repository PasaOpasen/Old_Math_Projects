using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category:IComparable
    {
        private string name;
        private MessageType type;
        private MessageTopic topic;
        public Category(string name,MessageType t= MessageType.Incoming,MessageTopic top= MessageTopic.Error)
        {
            this.name = name;
            this.type = t;
            this.topic = top;
        }

        public override string ToString()
        {
            return $"{name}.{type}.{topic}";
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
           Category c = (Category)obj;
            int res = string.Compare(name, c.name);          
            if (res != 0) return res;
            res = type.CompareTo(c.type);
            if (res != 0) return res;
            return topic.CompareTo(c.topic);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Category)) return false;
            Category c=(Category)obj;
            
            var category = (Category)obj;
            return 
                   name == category.name &&
                   type == category.type &&
                   topic == category.topic;
        }

        public override int GetHashCode()
        {
            var hashCode = 963374943;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            hashCode = hashCode * -1521134295 + topic.GetHashCode();
            return hashCode;
        }

        public static bool operator <(Category a, Category b) => a.CompareTo(b) == -1;
        public static bool operator <=(Category a, Category b) => a.CompareTo(b) <1;
        public static bool operator >(Category a, Category b) => a.CompareTo(b) == 1;
        public static bool operator >=(Category a, Category b) => a.CompareTo(b) > -1;
        public static bool operator ==(Category a, Category b) => a.CompareTo(b) == 0;
        public static bool operator !=(Category a, Category b) => a.CompareTo(b) !=0;
    }
}
