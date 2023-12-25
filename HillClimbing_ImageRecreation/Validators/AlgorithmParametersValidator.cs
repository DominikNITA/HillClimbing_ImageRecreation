using FluentValidation;
using Logic.Models;

namespace HillClimbing_ImageRecreation.Validators
{
    public class AlgorithmParametersValidator : AbstractValidator<AlgorithmParameters>
    {
        public AlgorithmParametersValidator()
        {
            RuleFor(x => x.MaxIterations).GreaterThan(0);

            RuleFor(x => x.Shapes).NotEmpty();

            RuleFor(x => x.ShapeSizeLimits.Min).GreaterThan(0);

            RuleFor(x => x.ShapeSizeLimits.Max).GreaterThanOrEqualTo(x => x.ShapeSizeLimits.Min);

            RuleFor(x => x.UseBackgroundColorChance).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1);

            RuleFor(x => x.BackgroundBaseColorString).Length(9);

            RuleFor(x => x.ColorDictParameters).NotNull().SetValidator(x => new ColorDictParametersValidator(x.MaxIterations));
        }

        class ColorDictParametersValidator : AbstractValidator<ColorDictParameters>
        {
            public ColorDictParametersValidator(int maxIterations) 
            {               
                When(x => x.Enabled, () =>
                {
                    RuleFor(x => x.StartUsingFromIteration).LessThanOrEqualTo(maxIterations).WithMessage("Max iterations cannot be greater than the starting iteration of color dict");
                    RuleFor(x => x.Resolution).GreaterThan(0);
                });
            }
        }
    }
}
