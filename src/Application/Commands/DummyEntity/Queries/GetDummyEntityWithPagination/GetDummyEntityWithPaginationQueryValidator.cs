namespace WebApiTemplate.Application.Commands.DummyEntity.Queries.GetDummyEntityWithPagination;

public class GetDummyEntityWithPaginationQueryValidator : AbstractValidator<GetDummyEntityWithPaginationQuery>
{
    public GetDummyEntityWithPaginationQueryValidator()
    {
        RuleFor(x => x.ListId)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
