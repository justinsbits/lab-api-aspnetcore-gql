using System;
using HotChocolate;

namespace CommanderGQL.GraphQL.Commands
{

    public class CommandNotFoundException : Exception
    {
        public int CommandId { get; internal set; }
    }
    public class CommandNotFoundExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is CommandNotFoundException ex)
                return error.WithMessage($"Command with Id '{ex.CommandId}' not found");
                
            return error;
        }
    }
}//"The LINQ expression 'DbSet<Tool>()\r\n    .Where(t => string.Equals(\r\n        a: t.Name, \r\n        b: __input_ToolName_0, \r\n        comparisonType: OrdinalIgnoreCase))' could not be translated. Additional information: Translation of the 'string.Equals' overload with a 'StringComparison' parameter is not supported. See https://go.microsoft.com/fwlink/?linkid=2129535 for more information. Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or 'ToListAsync'. See https://go.microsoft.com/fwlink/?linkid=2101038 for more information."