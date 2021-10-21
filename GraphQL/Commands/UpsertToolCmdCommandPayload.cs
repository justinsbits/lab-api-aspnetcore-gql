using CommanderDA.Entities;

namespace CommanderGQL.GraphQL.Tools
{
    public record UpsertToolCmdCommandPayload(Tool tool, Command command);
}