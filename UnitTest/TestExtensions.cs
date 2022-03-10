using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    internal static class TestExtensions
    {
        public static Mock<TRepo> Setup<TRepo, TResult>(this Mock<TRepo> mock,
        params ValueTuple<Expression<Func<TRepo, Task<TResult>>>, TResult>[] setup) where TRepo : class
        {
            foreach (var (method, retVal) in setup)
            {
                mock.Setup(method).ReturnsAsync(retVal);
            }

            return mock;
        }

        public static Mock<TRepo> Setup<TRepo, TEntity, TResult>(this Mock<TRepo> mock,
            params ValueTuple<Expression<Func<TRepo, Task<TResult>>>, Func<TEntity, TResult>>[] setup) where TRepo : class
        {
            foreach (var (method, retVal) in setup)
            {
                mock.Setup(method).ReturnsAsync(retVal);
            }

            return mock;
        }
    }
}
