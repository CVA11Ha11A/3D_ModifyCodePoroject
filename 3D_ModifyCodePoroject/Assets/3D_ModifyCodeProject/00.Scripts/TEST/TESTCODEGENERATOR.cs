//using System;
//using System.Text;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Text;

//[Generator]
//public class TESTCODEGENERATOR : ISourceGenerator
//{
//    public void Initialize(GeneratorInitializationContext context)
//    {
//        // 등록 등의 초기화 작업
//    }

//    public void Execute(GeneratorExecutionContext context)
//    {
//        // 이 부분에서 코드를 생성하고 컴파일러에게 제공
//        // 예를 들어, 클래스에 변수 추가하는 코드를 생성할 수 있음

//        // 생성된 코드를 컴파일러에게 제공
//        var sourceCode = @"
//using System;

//[AutoGenerateVariable(""GeneratedVariable"")]
//public class GeneratedClass
//{
//}
//";
//        context.AddSource("GeneratedClass", SourceText.From(sourceCode, Encoding.UTF8));
//    }
//}
