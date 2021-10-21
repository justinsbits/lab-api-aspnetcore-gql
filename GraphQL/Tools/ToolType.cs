using System.Linq;
using CommanderDA;
using CommanderDA.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL.Tools
{
    // Using Tool Model as basis for creating ToolType (via HotChocolates ObjectType)
    // Goal of ensuring API 'types' are not directly tied to internal models
    // e.g. can ensure any graphql specific info relates to types rathter than models (e.g. documentation annotations) 
   
    public class ToolType : ObjectType<Tool>
    {

        protected override void Configure(IObjectTypeDescriptor<Tool> descriptor)
        {
            descriptor.Description("Represents a software tool that has a command line interface.");

            descriptor
                .Field(p => p.Id)
                .Description("Represents the unique ID for the tool.");

            descriptor
                .Field(p => p.Name)
                .Description("Represents the name for the tool.");

            descriptor
                .Field(p => p.Description)
                .Description("Represents the description for the tool.");                

            //descriptor.Field(t => t.LicenseKey).Ignore(); <-- example of ignoring a field (don't expose through API)
            descriptor
                .Field(t => t.Commands)
                .ResolveWith<Resolvers>(t => t.GetCommands(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("List of available Commands for this Tool.");
        }

        private class Resolvers
        {
            public IQueryable<Command> GetCommands(Tool tool, [ScopedService] AppDbContext context)
            {
                var r = context.Commands.Where(c => c.ToolId == tool.Id);
                return r;
            }
        }
    }
}