using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using timelog.net.Controllers;
using timelog.net.Data;
using timelog.net.Models;

namespace UnitTest
{
    internal static class Repository
    {
        public static Mock<IRepository<ProjectTask>> Task<TResult>(params ValueTuple<Expression<Func<IRepository<ProjectTask>, Task<TResult>>>, TResult>[] setup) => Task().Setup(setup);

        public static Mock<IRepository<ProjectTask>> Task<TResult>(
            params ValueTuple<Expression<Func<IRepository<ProjectTask>, Task<TResult>>>, Func<ProjectTask, TResult>>[]
                setup) => Task().Setup(setup);

        public static Mock<IRepository<ProjectTask>> Task() => new();


        public static Mock<IEntryRepository> Entry<TResult>(
            Expression<Func<IEntryRepository, Task<TResult>>> method, TResult result) =>
            Entry((method, result));

        public static Mock<IEntryRepository> Entry<TResult>(
            params ValueTuple<Expression<Func<IEntryRepository, Task<TResult>>>, TResult>[] setup) =>
            Entry().Setup(setup);

        public static Mock<IEntryRepository> Entry<TResult>(
            params ValueTuple<Expression<Func<IEntryRepository, Task<TResult>>>, Func<Entry, TResult>>[] setup) =>
            Entry().Setup(setup);

        public static Mock<IEntryRepository> Entry() => new();
    }
}
