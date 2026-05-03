using FluentValidation;
using TalentSphere.DTOs.Selection;

namespace TalentSphere.Validators
{
    public class MakeSelectionDecisionValidator : AbstractValidator<MakeSelectionDecisionDTO>
    {
        public MakeSelectionDecisionValidator()
        {
            RuleFor(x => x.ApplicationID)
                .GreaterThan(0).WithMessage("A valid ApplicationID is required.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
                .When(x => x.Notes != null);

            RuleFor(x => x.Department)
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.")
                .When(x => x.Department != null);

            RuleFor(x => x.Position)
                .MaximumLength(200).WithMessage("Position cannot exceed 200 characters.")
                .When(x => x.Position != null);
        }
    }
}
