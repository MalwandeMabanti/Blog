using FluentValidation;
using Microsoft.AspNetCore.Http;
using ToDoList.Models;

public class BlogViewModelValidator : AbstractValidator<BlogViewModel>
{
    public BlogViewModelValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(15000).WithMessage("Description cannot be longer than 15000 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");
        });

        RuleSet("Update", () =>
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(15000).WithMessage("Description cannot be longer than 15000 characters.");
        });
    }
}
