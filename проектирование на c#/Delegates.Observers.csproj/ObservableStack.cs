using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    public delegate void HandleEvent(object eventData);

	public class StackOperationsLogger
	{
		private readonly Observer observer = new Observer();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(observer);
		}

		public string GetLog()
		{
			return observer.Log.ToString();
		}
	}

	public class Observer
	{
		public  StringBuilder Log = new StringBuilder();

        public HandleEvent HandleEvent;
        public Observer()
        {
            Log = new StringBuilder();
            HandleEvent = (object eventData) =>
          {
              if(!Log.ToString().EndsWith(eventData.ToString()))
              Log.Append(eventData);
          };
        }
	}

	public class ObservableStack<T>
	{
        public event HandleEvent HE= (object eventData)=> { };


        public void Add(Observer observer)
		{
            HE += observer.HandleEvent;
		}

		public void Notify(object eventData)
		{
            HE(eventData);
		}

		public void Remove(Observer observer)
		{
            HE -= observer.HandleEvent;
        }

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;

		}
	}
}
