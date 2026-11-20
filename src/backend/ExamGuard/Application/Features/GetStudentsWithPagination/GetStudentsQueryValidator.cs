using FluentValidation;

namespace Application.Features.GetStudentsWithPagination
{
    public class GetStudentsQueryValidator : AbstractValidator<GetStudentsQuery>
    {
        public GetStudentsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be at least 1.");
        }
    }
}
