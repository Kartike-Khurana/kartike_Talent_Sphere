using FluentValidation;
using TalentSphere.DTOs.Interview;

namespace TalentSphere.Validators
{
    public class ScheduleInterviewValidator : AbstractValidator<ScheduleInterviewDTO>
    {
        public ScheduleInterviewValidator()
        {
            RuleFor(x => x.ApplicationID)
                .GreaterThan(0).WithMessage("A valid ApplicationID is required.");

            RuleFor(x => x.InterviewerID)
                .GreaterThan(0).WithMessage("A valid InterviewerID is required.");

            RuleFor(x => x.Date)
                .Must(d => d >= DateOnly.FromDateTime(DateTime.UtcNow.Date))
                .WithMessage("Interview date cannot be in the past.");

            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.")
                .When(x => x.Location != null);
        }
    }
}
