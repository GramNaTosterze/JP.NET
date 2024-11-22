#include "DataModel.h"

using namespace Microsoft::CodeAnalysis;

DataModel::DataModel(const String^ codeText)
{
	_codeText = _codeText;
}

DataModel::NavigateToAddCodeCommand::NavigateToAddCodeCommand(DataModel^ viewModel)
{
    _viewModel = viewModel;
}

bool DataModel::NavigateToAddCodeCommand::CanExecute(System::Object^ parameter) 
{
    return true;
}

void DataModel::NavigateToAddCodeCommand::Execute(System::Object^ parameter)
{
    _viewModel->ErrorsList->Clear();
    _viewModel->ErrorsList->Add("Not implemented yet");
}


