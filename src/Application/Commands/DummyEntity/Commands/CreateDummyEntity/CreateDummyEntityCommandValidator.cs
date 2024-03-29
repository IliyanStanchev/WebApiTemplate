namespace WebApiTemplate.Application.Commands.DummyEntity.Commands.CreateDummyEntity;

public class CreateDummyEntityCommandValidator : AbstractValidator<CreateDummyEntityCommand>
{
    public CreateDummyEntityCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Matches("^[a-zA-Z0-9 ]*$")
            .WithMessage("Title must contain only letters, numbers and spaces.");
    }
}
