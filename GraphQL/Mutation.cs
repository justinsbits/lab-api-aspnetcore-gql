using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommanderDA;
using CommanderDA.Entities;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Tools;
using HotChocolate;
using HotChocolate.Data;

namespace CommanderGQL.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddToolPayload> AddToolAsync(AddToolInput input, [ScopedService] AppDbContext context)
        {
            var tool = new Tool{
                Name = input.Name
            };

            context.Tools.Add(tool);
            await context.SaveChangesAsync();
            return new AddToolPayload(tool);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] AppDbContext context)
        {
            var command = new Command{
                HowTo = input.HowTo,
                CommandLine = input.CommandLine,
                ToolId = input.ToolId
            };

            context.Commands.Add(command);
            await context.SaveChangesAsync();
            return new AddCommandPayload(command);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<UpsertToolCmdCommandPayload> UpsertToolCmdCommandAsync(UpsertToolCmdCommandInput input, [ScopedService] AppDbContext context)
        {
            // TODO - breakdown / add tests

            var command = context.Commands.Find(input.CommandId);
            if(command == null)
            {
                command = new Command{
                    HowTo = input.CommandDescription,
                    CommandLine = input.CommandLine,
                };
            }
            else
            {
                command.HowTo = input.CommandDescription;
                command.CommandLine = input.CommandLine;
                context.Commands.Update(command);
            }

            var toolId = command.ToolId > 0 ? command.ToolId : input.ToolId;
            var tool = context.Tools.Find(toolId);
            if (tool == null)
                tool = context.Tools.ToList().SingleOrDefault(t => string.Equals(t.Name, input.ToolName, StringComparison.OrdinalIgnoreCase));

            if (tool == null)
            {
                tool = new Tool
                {
                    Name = input.ToolName,
                    Description = input.ToolDescription
                };
                tool.Commands.Add(command);
                context.Tools.Add(tool);
            }
            else
            {
                tool.Name = input.ToolName;
                tool.Description = input.ToolDescription;
                if (command.Id > 0)
                    context.Tools.Update(tool);
                else
                    tool.Commands.Add(command);
            }

            await context.SaveChangesAsync();
            return new UpsertToolCmdCommandPayload(tool, command);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<DeleteCommandPayload> DeleteCommandAsync(DeleteCommandInput input, [ScopedService] AppDbContext context)
        {
            var command = context.Commands.Single(b => b.Id == input.Id);
            context.Commands.Remove(command);
            await context.SaveChangesAsync();
            return new DeleteCommandPayload(command);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<UpdateCommandPayload> UpdateCommandAsync(UpdateCommandInput input, [ScopedService] AppDbContext context)
        {
            var command = context.Commands.Find(input.Id); 
            if(command == null)
                throw new CommandNotFoundException() {CommandId = input.Id};
           
            command.HowTo = input.HowTo;
            command.CommandLine = input.CommandLine;

            context.Commands.Update(command);
            await context.SaveChangesAsync();
            return new UpdateCommandPayload(command);
        }
    }
}