using System.Linq;
using CommanderData;
using CommanderData.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL.Commands
{
    public class CommandType : ObjectType<Command>
    {
        protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
        {
            descriptor.Description("Represents an executable command.");

            descriptor
                .Field(c => c.Id)
                .Description("Represents the unique ID for the command.");

            descriptor
                .Field(c => c.HowTo)
                .Description("Represents the how-to for the command.");

            descriptor
                .Field(c => c.CommandLine)
                .Description("Represents the command line.");

            descriptor
                .Field(c => c.ToolId)
                .Description("Represents the unique ID of the tool which the command belongs.");

            descriptor
                .Field(c => c.Tool)
                .ResolveWith<Resolvers>(c => c.GetTool(default, default))
                .UseDbContext<AppDbContext>()
                .Description("Tool related to this command.");
        }

        private class Resolvers
        {
            public Tool GetTool(Command command, [ScopedService] AppDbContext context)
            {
                var r = context.Tools.FirstOrDefault(t => t.Id == command.ToolId);
                return r;
            }
        }        
    }
}