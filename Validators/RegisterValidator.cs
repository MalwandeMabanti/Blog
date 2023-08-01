//using FluentValidation;
//using Microsoft.AspNetCore.Identity;
//using ToDoList.Data;
//using ToDoList.Models;

//public class RegisterValidator : AbstractValidator<Register>
//{

//    public RegisterValidator()
//    {
//        RuleFor(x => x.Email)
//            .NotEmpty().WithMessage("Email is required.")
//            .EmailAddress().WithMessage("Email is not valid.");

//        //RuleFor(x => x.Password)
//        //    .NotEmpty().WithMessage("Password is required.")
//        //    .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

//        RuleFor(x => x.FirstName)
//            .NotEmpty().WithMessage("First name is required.")
//            .Length(1, 50).WithMessage("First name must be between 1 and 50 characters.");

//        RuleFor(x => x.LastName)
//            .NotEmpty().WithMessage("Last name is required.")
//            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters.");
//    }
//    /*

//public class RegisterValidator : AbstractValidator<Register>
//{
//    public RegisterValidator()
//    {
//        RuleFor(x => x.Email)
//            .NotEmpty().WithMessage("Email is required.")
//            .EmailAddress().WithMessage("Email is not valid.");

//        RuleFor(x => x.Password)
//            .NotEmpty().WithMessage("Password is required.")
//            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

//        RuleFor(x => x.FirstName)
//            .NotEmpty().WithMessage("First name is required.")
//            .Length(1, 50).WithMessage("First name must be between 1 and 50 characters.");

//        RuleFor(x => x.LastName)
//            .NotEmpty().WithMessage("Last name is required.")
//            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters.");
//    }
//} 
     
//     */
//}


