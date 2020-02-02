using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.API
{
    class APIObject : IDisposable
    {
        List<int> list=new List<int>();

        public APIObject(int i)
        {
            MagicAPI.Allocate(i);
            list.Add(i);
        }

        private bool isDisposed = false;

        ~APIObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //финализатор не будет вызываться
        }

        protected virtual void Dispose(bool fromDisposeMethod)
        {
            if (!isDisposed)
            {
                if (fromDisposeMethod)
                {
                    //list = null;
                }
                foreach (int i in list)
                    MagicAPI.Free(i);
                //Console.WriteLine("Очистка неуправляемых ресурсов в {0}", name);
                isDisposed = true;
                // base.Dispose(isDisposing); // если унаследован от Disposable класса
            }
        }
    }
}
