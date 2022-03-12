using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using timelog.net.Data;

namespace UnitTest
{
    internal static class Repository
    {
        public static Mock<IRepository<TEntity>> Repo<TEntity, TResult>(params ValueTuple<Expression<Func<IRepository<TEntity>, Task<TResult>>>, TResult>[] setup) => Repo<TEntity>().Setup(setup);

        public static Mock<IRepository<TEntity>> Repo<TEntity, TResult>(
            params ValueTuple<Expression<Func<IRepository<TEntity>, Task<TResult>>>, Func<TEntity, TResult>>[]
                setup) => Repo<TEntity>().Setup(setup);

        public static Mock<IRepository<TEntity>> Repo<TEntity>() => new();
    }
}
