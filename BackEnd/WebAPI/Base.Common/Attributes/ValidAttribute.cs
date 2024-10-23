namespace Base.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidStringAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidNumberAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidPhoneAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidEmailAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidDateAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidNumberMaxAttribute : Attribute
    {
        public int MaxValue { get; }
        public ValidNumberMaxAttribute(int value)
        {
            MaxValue = value;
        }
    }
}
