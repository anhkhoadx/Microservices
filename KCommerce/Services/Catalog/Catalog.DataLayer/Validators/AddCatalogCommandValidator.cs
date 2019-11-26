using Catalog.DataLayer.Commands;
using FluentValidation;

namespace Catalog.DataLayer.Validators
{
	public class AddCatalogCommandValidator : AbstractValidator<AddCatalogCommand>
	{
		public AddCatalogCommandValidator()
		{
			RuleFor(m => m.Name).NotNull().NotEmpty();
			RuleFor(m => m.Description).NotNull().NotEmpty();
			RuleFor(m => m.PictureBytes).NotNull();
			RuleFor(m => m.AvailableStock).GreaterThan(0);
			RuleFor(m => m.Price).GreaterThan(0);
		}
	}
}
