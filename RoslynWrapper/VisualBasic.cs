using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace RoslynWrapper
{
    public class VisualBasic
    {
        private static VisualBasicCompilation Create(string assemblyName, IEnumerable<SyntaxTree> syntaxTrees,
            IEnumerable<MetadataReference> metadataReferences, VisualBasicCompilationOptions VBCompilationOptions)
        {
            return VisualBasicCompilation.Create(assemblyName, syntaxTrees, metadataReferences, VBCompilationOptions);
        }
        public static List<string> Compile(string dllPath, string assemblyName, IEnumerable<SyntaxTree> syntaxTrees,
            IEnumerable<MetadataReference> metadataReferences, VisualBasicCompilationOptions VBCompilationOptions)
        {
            var compilation = Create(assemblyName, syntaxTrees, metadataReferences, VBCompilationOptions);
            var result = compilation.Emit(dllPath);
            return result.Success ? [] : result.Diagnostics.Select(d => d.ToString()).ToList();
        }
    }
}
