using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace OpenRoutePlanner.Components.FormControls;

public class NullableGuidInputSelect : InputSelect<Guid?>
{
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out Guid? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = null;
        validationErrorMessage = null;
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        if (Guid.TryParse(value, out var g))
        {
            result = g;
            return true;
        }

        validationErrorMessage = "The selected valid is not a valid GUID";
        return false;
    }
}
