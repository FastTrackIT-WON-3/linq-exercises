using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace LinqExercises
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaComparer;

        public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer)
        {
            _lambdaComparer = lambdaComparer;
        }

        public bool Equals(T? x, T? y)
        {
            if (_lambdaComparer is null)
            {
                return false;
            }

            return _lambdaComparer.Invoke(x, y);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return HashCode.Combine(obj);
        }
    }
}
