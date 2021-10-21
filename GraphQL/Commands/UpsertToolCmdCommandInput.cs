namespace CommanderGQL.GraphQL.Commands
{
    public record UpsertToolCmdCommandInput(int? ToolId, string ToolName, string ToolDescription, int? CommandId, string CommandDescription, string CommandLine);
}