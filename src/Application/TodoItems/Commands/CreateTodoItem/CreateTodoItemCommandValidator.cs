namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.");

        RuleFor(v => v.DueDate)
            .Must(d => d!.Value.Date >= DateTime.Today)
            .When(v => v.DueDate.HasValue)
            .WithMessage("Due date cannot be in the past.");
    }
}
