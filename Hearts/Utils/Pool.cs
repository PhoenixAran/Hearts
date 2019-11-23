using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Utils
{
    public static class Pool<T> where T : new()
    {
        private static readonly Queue<T> _pool = new Queue<T>( 24 );

        public static T Obtain()
        {
            if ( _pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            return new T();
        }

        public static void Free(T element)
        {
            _pool.Enqueue( element );
            if (element is IPoolable poolableObj)
            {
                poolableObj.Reset();
            }
        }


    }
}
