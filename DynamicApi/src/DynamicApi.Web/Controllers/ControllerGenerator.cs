using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using DynamicApi.Web.Models;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace DynamicApi.Web.Controllers
{
    public class ControllerGenerator
    {
        private SyntaxTree Code()
        {
            string body = Source();
            return CSharpSyntaxTree.ParseText($"using Microsoft.AspNet.Mvc;namespace DynamicApi.Web.Controllers {{ {body} }}");
        }

        private static string Source()
        {
            var type = typeof(IId);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            var declarations = types.Select(x => "[Route(\"api/[controller]\")] \r\n" +
                $"public class {x.Name}Controller : Controller<{x.FullName}> {{ " +
                $"public ValuesController(DynamicApi.Web.Repositories.IRepository<{x.FullName}> repository) : base(repository) {{ }} }}"
            );

            var body = string.Join("\r\n", declarations);
            return body;
        }
        
        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(RouteAttribute).Assembly.Location),
            //MetadataReference.CreateFromFile(typeof(IId).Assembly.ManifestModule.ToString())
        };

        public void Load()
        {
            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { Code() },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                }
            }
        }
    }
}
