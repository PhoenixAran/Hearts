using System;
using System.Collections.Generic;
using System.Text;

namespace Hearts.Core
{
    public static class ListPool<T>
    {
        private static readonly Queue<List<T>> _pool = new Queue<List<T>>( 24 );

        public static List<T> Obtain()
        {
            if ( _pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            return new List<T>();
        }

        public static void Free(List<T> list)
        {
            _pool.Enqueue( list );
            list.Clear();
        }
    }
}
