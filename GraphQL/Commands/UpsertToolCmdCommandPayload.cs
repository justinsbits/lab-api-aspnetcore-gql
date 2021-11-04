using CommanderData.Entities;

namespace CommanderGQL.GraphQL.Tools
{
    public record UpsertToolCmdCommandPayload(Tool tool, Command command);
}