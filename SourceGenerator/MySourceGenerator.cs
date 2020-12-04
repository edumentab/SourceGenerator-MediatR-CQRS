using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceGenerator
{
    /// <summary>
    /// Source generator that will generate the necessary API controllers and action to handle the commands and queries int the system.
    /// Written by Tore Nestenius, https://www.edument.se
    /// 
    /// </summary>
    [Generator]
    public class MySourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a factory that can create our custom syntax receiver
            context.RegisterForSyntaxNotifications(() => new MySyntaxReceiver());
        }

        /// <summary>
        /// Called to perform source generation. 
        /// </summary>
        /// <param name="context"></param>
        public void Execute(GeneratorExecutionContext context)
        {
            MySyntaxReceiver syntaxReceiver = (MySyntaxReceiver)context.SyntaxReceiver;
            GenerateCommandClass(context, syntaxReceiver);
            GenerateQueryClass(context, syntaxReceiver);
        }

        /// <summary>
        /// Generate the API controller class to handle the query commands
        /// </summary>
        /// <param name="context"></param>
        /// <param name="syntaxReceiver"></param>
        private void GenerateQueryClass(GeneratorExecutionContext context, MySyntaxReceiver syntaxReceiver)
        {
            string QueryClassTemplate = LoadTemplate(context, filename: "QueryClassTemplate.txt");

            StringBuilder commandSource = new StringBuilder();

            foreach (var query in syntaxReceiver.Queries)
            {
                var queryCommandName = query.Identifier.ValueText;   
                var queryReturnType = LookupIRequestGenericType(query);
                var commandComments = query.GetLeadingTrivia().ToString();

                commandSource.AppendLine(@$"
        {commandComments}
        /// <param name=""command"">An instance of the {queryCommandName}</param>
        /// <returns>The returned result of this command</returns>
        /// <response code=""201"">Returns the newly created item</response>
        /// <response code=""400"">If the item is null</response>   
        [HttpPost]
        [Produces(""application/json"")]
        [ProducesResponseType(typeof({queryReturnType}), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<{queryReturnType}> {queryCommandName}([FromBody]{queryCommandName} command)
                {{
                    return await _mediator.Send(command);
                }}

                ");
            }

            string finalSource = QueryClassTemplate.Replace("###Queries###", commandSource.ToString());

            SourceText sourceText = SourceText.From(finalSource, Encoding.UTF8);

            // inject the created source into the users compilation
            context.AddSource("GeneratedQueryController.cs", sourceText);
        }


        /// <summary>
        /// Generate the API Controller class for the commands
        /// </summary>
        /// <param name="context"></param>
        /// <param name="syntaxReceiver"></param>
        private void GenerateCommandClass(GeneratorExecutionContext context, MySyntaxReceiver syntaxReceiver)
        {
            StringBuilder commandSource = new StringBuilder();
            string CommandClassTemplate = LoadTemplate(context, filename: "CommandClassTemplate.txt");

            foreach (var command in syntaxReceiver.Commands)
            {
                var commandName = command.Identifier.ValueText;
                var commandReturnType = LookupIRequestGenericType(command);
                var commandComments = command.GetLeadingTrivia().ToString();

                commandSource.AppendLine(@$"

        {commandComments}
        /// <param name=""command"">An instance of the {commandName}</param>
        /// <returns>The status of the operation</returns>
        /// <response code=""201"">Returns the newly created item</response>
        /// <response code=""400"">If the item is null</response>   
        [HttpPost]
        [Produces(""application/json"")]
        [ProducesResponseType(typeof({commandReturnType}), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<{commandReturnType}> {commandName}([FromBody]{commandName} command)
                {{
                    return await _mediator.Send(command);
                }}

                ");
            }

            string finalSource = CommandClassTemplate.Replace("###Commands###", commandSource.ToString());

            SourceText sourceText = SourceText.From(finalSource, Encoding.UTF8);

            // inject the created source into the users compilation
            context.AddSource("GeneratedCommandController.cs", sourceText);
        }


        /// <summary>
        /// Lookup the genric type of the IRequest interface (ie the return type of the command)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private string LookupIRequestGenericType(TypeDeclarationSyntax command)
        {
            foreach (var entry in command.BaseList.Types)
            {
                if (entry is SimpleBaseTypeSyntax basetype)
                {
                    if (basetype.Type is GenericNameSyntax type)
                    {
                        if ((type.Identifier.ValueText == "ICommand" || type.Identifier.ValueText == "IQuery") &&
                                   type.TypeArgumentList.Arguments.Count == 1)
                        {
                            return type.TypeArgumentList.Arguments[0].ToString();
                        }
                    }
                }
            }
            return "";
        }


        /// <summary>
        /// Load the external template
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filename">The filename to look for, case-sensitive</param>
        /// <returns>The content of the file</returns>
        private string LoadTemplate(GeneratorExecutionContext context, string filename)
        {
            var addditionalfile = context.AdditionalFiles.FirstOrDefault(x => x.Path.EndsWith(filename));

            if (addditionalfile != null)
            {
                return File.ReadAllText(addditionalfile.Path);
            }
            else
                throw new FileNotFoundException("The file " + filename + " was not found");
        }
    }


    internal class MySyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// List of matching commands that we want to generate code for (classes/records implementing ICommand<T>
        /// </summary>
        public List<TypeDeclarationSyntax> Commands { get; private set; } = new List<TypeDeclarationSyntax>();

        /// <summary>
        /// List of matching query commands that we want to generate code for (classes/records implementing IQuery<T>
        /// </summary>
        public List<TypeDeclarationSyntax> Queries { get; private set; } = new List<TypeDeclarationSyntax>();

        /// <summary>
        /// Called for each SyntaxNode in the compilation
        /// </summary>
        /// <param name="syntaxNode">The current SyntaxNode being visited</param>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Identify if the class/record implements the ICommand<T> or IQuery<T> interface
            if (syntaxNode is ClassDeclarationSyntax || syntaxNode is RecordDeclarationSyntax)
            {
                var tds = (TypeDeclarationSyntax)syntaxNode;

                if (tds.BaseList != null)
                {
                    var baselist = tds.BaseList;

                    foreach (var entry in baselist.Types)
                    {
                        if (entry is SimpleBaseTypeSyntax basetype)
                        {
                            if (basetype.Type is GenericNameSyntax type)
                            {
                                if (type.Identifier.ValueText == "ICommand" &&
                                    type.TypeArgumentList.Arguments.Count == 1 &&
                                    (type.TypeArgumentList.Arguments[0] is TypeSyntax))
                                {
                                    Commands.Add(tds);
                                    return;
                                }

                                if (type.Identifier.ValueText == "IQuery" &&
                                    type.TypeArgumentList.Arguments.Count == 1 &&
                                    (type.TypeArgumentList.Arguments[0] is TypeSyntax))
                                {
                                    Queries.Add(tds);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}