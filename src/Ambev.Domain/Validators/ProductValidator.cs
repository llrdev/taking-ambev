﻿using Ambev.Domain.Entities;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Domain.Validators;

[ExcludeFromCodeCoverage]
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotNull()
            .WithMessage("Product name is required.")
            .NotEmpty()
            .WithMessage("Product name cannot be empty.")
            .MaximumLength(150)
            .WithMessage("Product name must not exceed 150 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(500)
            .WithMessage("Product description must not exceed 500 characters.");

        RuleFor(p => p.Category)
            .IsInEnum()
            .WithMessage("Invalid product category.");

        RuleFor(p => p.BasePrice)
            .GreaterThan(0)
            .WithMessage("Base price must be greater than zero.");
    }
}