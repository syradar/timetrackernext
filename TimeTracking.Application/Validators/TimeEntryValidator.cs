using FluentValidation;
using TimeTracking.Application.Models;

namespace TimeTracking.Application.Validators;

public class TimeEntryValidator : AbstractValidator<TimeEntry>
{
    public TimeEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.Hours).NotEmpty();
        RuleFor(x => x.Comments).NotEmpty();
    }
}