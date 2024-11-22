#include <windows.h>
using namespace System;

using namespace System::Reflection;
using namespace System::Security;
using namespace System::Runtime::Remoting;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::ComponentModel;
using namespace System::Windows::Input;
using namespace Microsoft::CodeAnalysis;

typedef ObservableCollection<MethodInfo^> myList;

ref class DataModel : INotifyPropertyChanged {

#pragma region Private Fields

	String^ _parameterText;
	String^ _codeText;
	String^ _invokeResultText;
	String^ _intFieldText;
	String^ _boolFieldText;
	ICollection<String^>^ _errorsList = gcnew ObservableCollection<String^>();
	ICollection<String^>^ _methodsList = gcnew ObservableCollection<String^>();
	String^ selectedMethod = nullptr;
	ICollection<String^>^ _fieldsList = gcnew ObservableCollection<String^>();
	String^ selectedField = nullptr;
	OutputKind^ selectedOutputKind = OutputKind::DynamicallyLinkedLibrary;
	array<MethodInfo^>^ methods = nullptr;
	array<FieldInfo^>^ fields = nullptr;
	Object^ objectInstance;
	System::Type^ type;
	ObjectHandle^ handle;
	ICommand^ _AddCodeCommand;
	ICommand^ _InvokeCommand;
	ICommand^ _SetCommand;

#pragma endregion End of Private Fields

public:

#pragma region Public Constructors

	DataModel(const String^ codeText);

#pragma endregion End of Public Constructors

#pragma region Public Events

	virtual event System::ComponentModel::PropertyChangedEventHandler^ PropertyChanged; /*{
		virtual void add(PropertyChangedEventHandler^ value) sealed =
			INotifyPropertyChanged::PropertyChanged::add{ }
		void remove(PropertyChangedEventHandler^ value)  sealed =
			INotifyPropertyChanged::PropertyChanged::remove{ }
	}*/

#pragma endregion End of Public Events

#pragma region Public Properties

	property String^ ParameterText{
		String ^ get() {
			return _parameterText;
		}

		void set(String ^ value) {
			_parameterText = value;
		}
	}

	property String^ CodeText {
		String^ get() {
			return _codeText;
		}

		void set(String^ value) {
			_codeText = value;
		}
	}

	property String^ InvokeResultText{
		String ^ get() {
			return _invokeResultText;
		}

		void set(String ^ value) {
			_invokeResultText = value;
		}
	}
	property String^ IntFieldText{
		String ^ get() {
			return _intFieldText;
		}
		void set(String ^ value) {
			_intFieldText = value;
		}
	}
	property String^ BoolFieldText{
		String ^ get() {
			return _boolFieldText;
		}
		void set(String ^ value) {
			_boolFieldText = value;
		}
	}

	property ICollection<String^>^ ErrorsList {
		ICollection<String^>^ get() {
			return _errorsList;
		}

		void set(ICollection<String^>^ value) {
			_errorsList = value;
		}
	}

	property ICollection<String^>^ MethodsList {
		ICollection<String^>^ get() {
			return _methodsList;
		}

		void set(ICollection<String^>^ value) {
			_methodsList = value;
		}
	}

	property ICollection<String^>^ FieldsList {
		ICollection<String^>^ get() {
			return _fieldsList;
		}

		void set(ICollection<String^>^ value) {
			_fieldsList = value;
		}
	}

	property ICommand^ AddCodeCommand {
		ICommand^ get() {
			if (_AddCodeCommand == nullptr)
			{
				_AddCodeCommand = gcnew NavigateToAddCodeCommand(this);
			}
			return _AddCodeCommand;
		}
		void set(ICommand^ value) {
			_AddCodeCommand = value;
		}
	}
	property ICommand^ InvokeCommand {
		ICommand^ get() {
			if (_InvokeCommand == nullptr)
			{
				_InvokeCommand = gcnew NavigateToAddCodeCommand(this);
			}
			return _InvokeCommand;
		}
		void set(ICommand^ value) {
			_InvokeCommand = value;
		}
	}
	property ICommand^ SetCommand {
		ICommand^ get() {
			if (_SetCommand == nullptr)
			{
				_SetCommand = gcnew NavigateToAddCodeCommand(this);
			}
			return _SetCommand;
		}
		void set(ICommand^ value) {
			_SetCommand = value;
		}
	}


	property OutputKind^ SelectedOutputKind {
		OutputKind^ get() {
			return selectedOutputKind;
		}

		void set(OutputKind^ value) {
			selectedOutputKind = value;
		}
	}
	property String^ SelectedMethod {
		String^ get() {
			return selectedMethod;
		}

		void set(String^ value) {
			selectedMethod = value;
		}
	}
	property String^ SelectedField {
		String^ get() {
			return selectedField;
		}

		void set(String^ value) {
			selectedField = value;
		}
	}
	property array<MethodInfo^>^ Methods {
		array<MethodInfo^>^ get() {
			return methods;
		}

		void set(array<MethodInfo^>^ value) {
			methods = value;
		}
	}

	property array<FieldInfo^>^ Fields {
		array<FieldInfo^>^ get() {
			return fields;
		}

		void set(array<FieldInfo^>^ value) {
			fields = value;
		}
	}

	property Object^ ObjectInstance {
		Object^ get() {
			return objectInstance;
		}

		void set(Object^ value) {
			objectInstance = value;
		}
	}

	property ObjectHandle^ Handle {
		ObjectHandle^ get() {
			return handle;
		}

		void set(ObjectHandle^ value) {
			handle = value;
		}
	}

	property System::Type^ Type {
		System::Type^ get() {
			return type;
		}

		void set(System::Type^ value) {
			type = value;
		}
	}

#pragma endregion End of Public Events

#pragma region Public Nested Classes

	ref class NavigateToAddCodeCommand : public ICommand {

		DataModel^ _viewModel;
		
		property DataModel^ ViewModel {
			DataModel^ get() {
				return _viewModel;
			}
		
			void set(DataModel^ value) {
				_viewModel = value;
			}
		}
		virtual bool CanExecute(System::Object^ parameter) = ICommand::CanExecute;
		virtual void Execute(System::Object^ parameter) = ICommand::Execute;
	public:

		NavigateToAddCodeCommand(DataModel^ viewModel);
		virtual event EventHandler^ CanExecuteChanged {
			void add(EventHandler^) {}
			void remove(EventHandler^) {}
		}
	};

#pragma endregion End of Public Nested Classes
	public:
		void NotifyPropertyChanged() {
			PropertyChanged(this, gcnew PropertyChangedEventArgs(""));
		}
};

