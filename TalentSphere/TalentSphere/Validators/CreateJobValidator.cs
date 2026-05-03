using FluentValidation;
using TalentSphere.DTOs;

namespace TalentSphere.Validators
{
    public class CreateJobValidator : AbstractValidator<CreateJobDTO>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.");

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Requirements are required.")
                .MaximumLength(5000).WithMessage("Requirements cannot exceed 5000 characters.");
        }
    }
}
