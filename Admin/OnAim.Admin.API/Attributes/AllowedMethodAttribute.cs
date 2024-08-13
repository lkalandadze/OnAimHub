namespace OnAim.Admin.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AllowedMethodAttribute : Attribute
    {
        public AllowedMethodAttribute()
        {
        }
    }
}
