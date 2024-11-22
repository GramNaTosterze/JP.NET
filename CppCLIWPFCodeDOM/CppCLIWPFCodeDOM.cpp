#include <windows.h>
#include "DataModel.h"
using namespace System;
using namespace System::Windows;
using namespace System::IO;
using namespace System::Windows::Markup;
using namespace System::Windows::Controls;
using namespace System::Reflection;
using namespace System::Windows::Documents;
using namespace Microsoft::CodeAnalysis;
using namespace Microsoft::CodeAnalysis::VisualBasic;
using namespace Microsoft::CodeAnalysis::Text;

using namespace std;

public ref class MyApplication : public Application
{
    DataModel^ h_dm;
    String^ pathToVBCode, ^ code;
    String^ dllPath = gcnew String(Path::Combine(System::IO::Directory::GetCurrentDirectory(), "VBCode.dll"));

    public: MyApplication(Window^ win)
    {
        pathToVBCode = gcnew String("..\\..\\..\\..\\VBCode\\Class1.vb");
        code = File::ReadAllText(pathToVBCode);
        h_dm = gcnew DataModel(code);
        win->DataContext = h_dm;

        h_dm->AddCodeCommand = gcnew CommandWrapper(this);
        h_dm->InvokeCommand = gcnew InvokeCommandWrapper(this);
		h_dm->SetCommand = gcnew SetCommandWrapper(this);

        h_dm->CodeText = code;
    }    

          void VBCompile() 
		  {
			  h_dm->ErrorsList->Clear();

              auto syntaxTree = VisualBasicSyntaxTree::ParseText(SourceText::From(h_dm->CodeText, System::Text::Encoding::UTF8, SourceHashAlgorithm::Sha1), VisualBasicParseOptions::Default, pathToVBCode, System::Threading::CancellationToken());
              auto syntaxTrees = gcnew array<SyntaxTree^> { syntaxTree };

              auto assemblyPath = gcnew String("C:\\Program Files\\dotnet\\packs\\Microsoft.NETCore.App.Ref\\8.0.11\\ref\\net8.0");
              auto metadataReferences = gcnew array<MetadataReference^> {
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.Runtime.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.Console.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.Text.Encoding.Extensions.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "Microsoft.VisualBasic.Core.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.IO.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
				  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "mscorlib.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
                  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default   ),
				  MetadataReference::CreateFromFile(Path::Combine(assemblyPath, "System.Core.dll"), MetadataReferenceProperties::Assembly, DocumentationProvider::Default),
              };

              auto kv = gcnew array<KeyValuePair<String^, ReportDiagnostic>> {};
              XmlReferenceResolver^ xmlReferenceResolver;
              SourceReferenceResolver^ sourceReferenceResolver;
              MetadataReferenceResolver^ metadataReferenceResolver;
              AssemblyIdentityComparer^ assemblyIdentityComparer;
              StrongNameProvider^ strongNameProvider;


              auto VBCompilationOptions = gcnew VisualBasicCompilationOptions(*h_dm->SelectedOutputKind,
                  nullptr, nullptr, gcnew String("Script"), GlobalImport::Parse(gcnew array<String^> {"System", "System.IO", "System.Text"}),
                  gcnew String("VBCode"), OptionStrict::Off, true, true, false, VisualBasicParseOptions::Default, false,
                  OptimizationLevel::Debug, true, nullptr, nullptr, System::Collections::Immutable::ImmutableArray<byte>(),
                  Nullable<bool>(), Platform::AnyCpu, ReportDiagnostic::Default, kv,
                  true, false, xmlReferenceResolver, sourceReferenceResolver, metadataReferenceResolver,
                  assemblyIdentityComparer, strongNameProvider, false, false, MetadataImportOptions::Public
              );

              
              for each(String^ error in RoslynWrapper::VisualBasic::Compile(dllPath, "VBCode", syntaxTrees, metadataReferences, VBCompilationOptions))
              {
				  h_dm->ErrorsList->Add(error);
              }
          }

          void Reflect() {
              try
              {
                  auto vb = Assembly::LoadFile(dllPath);

                  h_dm->ObjectInstance = vb->CreateInstance("VBCode.A");
                  h_dm->Type = h_dm->ObjectInstance->GetType();

                  h_dm->Fields = h_dm->Type->GetFields();
                  h_dm->FieldsList->Clear();
                  UpdateFields();

                  h_dm->Methods = h_dm->Type->GetMethods();
                  h_dm->MethodsList->Clear();
                  for each (auto method in h_dm->Methods)
                  {
                      h_dm->MethodsList->Add(method->Name);
                  }

                  h_dm->Handle = gcnew ObjectHandle(h_dm->ObjectInstance);
              }
              catch (Exception^ ex)
              {
                  h_dm->ErrorsList->Add("Cannot load dll");
              }
          }

          void UpdateFields() 
          {
			  h_dm->FieldsList->Clear();
              for each(auto field in h_dm->Fields)
              {
                  h_dm->FieldsList->Add(String::Format("{0}: {1} = {2}", field->Name, field->GetModifiedFieldType(), field->GetValue(h_dm->ObjectInstance)));
              }
          }

          void InvokeMethod() 
          {
              auto method = h_dm->SelectedMethod;
              MethodInfo^ mth = h_dm->Type->GetMethod(method);

              array<Object^>^ params;

              if (mth->GetParameters()->Length > 0) {
                  params = gcnew array<Object^> { h_dm->ParameterText };
              } else {
                  params = gcnew array<Object^>{};
              };

              Object^ retVal = mth->Invoke(h_dm->ObjectInstance, params);
              h_dm->InvokeResultText = retVal->ToString();

              
              h_dm->NotifyPropertyChanged();
              UpdateFields();
              
          }

          ref class CommandWrapper : public ICommand
          {
			  MyApplication^ _app;
          public:
              CommandWrapper(MyApplication^ app) : _app(app) {}

              virtual bool CanExecute(System::Object^ parameter)
              {
				  return true;
              };
              virtual void Execute(System::Object^ parameter)
              {
                  _app->VBCompile();
                  _app->Reflect();
              };
              virtual event EventHandler^ CanExecuteChanged{
                  void add(EventHandler^) {}
                  void remove(EventHandler^) {}
              }

          };

          ref class SetCommandWrapper : public ICommand
          {
              MyApplication^ _app;
          public:
              SetCommandWrapper(MyApplication^ app) : _app(app) {}

              virtual bool CanExecute(System::Object^ parameter)
              {
                  return true;
              };
              virtual void Execute(System::Object^ parameter)
              {
                  try 
                  {
                      auto field_val = Int32::Parse(_app->h_dm->IntFieldText);
                      _app->h_dm->ObjectInstance->GetType()->GetField("i")->SetValue(_app->h_dm->ObjectInstance, field_val);
                  }
                  catch (Exception^ ex) 
                  {
					  _app->h_dm->ErrorsList->Add(String::Format("{0}", ex));
                  }

                  try
                  {
                      auto field_val = Boolean::Parse(_app->h_dm->BoolFieldText);
                      _app->h_dm->ObjectInstance->GetType()->GetField("b")->SetValue(_app->h_dm->ObjectInstance, field_val);
                  }
                  catch (Exception^ ex)
                  {
					  _app->h_dm->ErrorsList->Add(String::Format("{0}", ex));
                  }
                  _app->UpdateFields();

              };
              virtual event EventHandler^ CanExecuteChanged{
                  void add(EventHandler^) {}
                  void remove(EventHandler^) {}
              }

          };

          ref class InvokeCommandWrapper : public ICommand
          {
              MyApplication^ _app;
          public:
              InvokeCommandWrapper(MyApplication^ app) : _app(app) {}

              virtual bool CanExecute(System::Object^ parameter)
              {
                  return true;
              };
              virtual void Execute(System::Object^ parameter)
              {
                  Current->Dispatcher->Invoke(gcnew Action(_app, &MyApplication::InvokeMethod));
              };
              virtual event EventHandler^ CanExecuteChanged {
                  void add(EventHandler^) {}
                  void remove(EventHandler^) {}
              }

          };
};

static public ref class Start
{
public:
    static void WinMain()
    {
        Stream^ st = File::OpenRead("MainWindow.xaml");
        Window^ win = (Window^)XamlReader::Load(st, nullptr);
        Application^ app = gcnew MyApplication(win);
        app->Run(win);
    }
};
