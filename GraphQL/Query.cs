using System.Linq;
using CommanderDA;
using CommanderDA.Entities;
using HotChocolate;
using HotChocolate.Data;

namespace CommanderGQL.GraphQL
{
    public class Query
    {
        // notes:
        // * DI method injection here
        // * Convention is to preface resource being requested with "Get" 
        // * [UseDbContext(typeof(AppDbContext))] - UseDbContext indicating getting context from pool (see services.AddPooledDbContextFactory<AppDbContext>... in startup.cs)
        // * GetTool([ScopedService]... - service lifetime set to "ScopedService" (create once per client request, rather than Singleton - same for all, or Transient - new each time)
        // * [UseProjection] - walk the graph to pull back child objects
        [UseDbContext(typeof(AppDbContext))]
        //[UseProjection] //<-- with resolves implemented do not need
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tool> GetTool([ScopedService] AppDbContext context)
        {
            return context.Tools;

        }

        [UseDbContext(typeof(AppDbContext))]
        //[UseProjection] //<-- with resolves implemented do not need
        [UseFiltering]
        [UseSorting]
        public IQueryable<Command> GetCommand([ScopedService] AppDbContext context)
        {
            return context.Commands;

        }
    }
}