namespace OnAim.Admin.Shared.ApplicationInfrastructure
{
    public class ValidationHandlerArg<TTarget, AArg>
    {
        public TTarget Target { get; private set; }
        public AArg Argument { get; private set; }

        public ValidationHandlerArg(TTarget target, AArg argument)
        {
            Target = target;
            Argument = argument;
        }
    }
}
