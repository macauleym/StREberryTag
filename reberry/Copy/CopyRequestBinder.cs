using System.CommandLine;
using System.CommandLine.Binding;

namespace reberry;

public class CopyRequestBinder(
  Argument<string> folderPathArgument
, Option<string> coverPathOption) : BinderBase<CopyRequest>
{
    protected override CopyRequest GetBoundValue(BindingContext bindingContext) =>
        new(
              bindingContext.ParseResult.GetValueForArgument(folderPathArgument)
            , bindingContext.ParseResult.GetValueForOption(coverPathOption)
            );
}
