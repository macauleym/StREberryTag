using System.CommandLine;
using System.CommandLine.Binding;

namespace reberry;

public class UpdateRequestBinder(
  Argument<string> pathArgument
, Option<string> commentOption
, Option<string> genreOption
, Option<string> copyrightOption
, Option<int> releasedOption
, Option<int> trackTotalOption
, Option<int> discTotalOption
, Option<string> coverOption
) : BinderBase<UpdateRequest>
{
    T GetOptionValue<T>(BindingContext fromContext, Option<T> ofOption) =>
        fromContext.ParseResult.GetValueForOption(ofOption);
    
    protected override UpdateRequest GetBoundValue(BindingContext bindingContext) =>
        new(
          bindingContext.ParseResult.GetValueForArgument(pathArgument)
        , GetOptionValue(bindingContext, commentOption)
        , GetOptionValue(bindingContext, genreOption)
        , GetOptionValue(bindingContext, copyrightOption)
        , GetOptionValue(bindingContext, releasedOption)
        , GetOptionValue(bindingContext, trackTotalOption)
        , GetOptionValue(bindingContext, discTotalOption)
        , GetOptionValue(bindingContext, coverOption)
        );
}
