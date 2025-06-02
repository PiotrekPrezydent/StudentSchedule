using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForeignKeyOf : Attribute
    {
        public Type TargetEntityType { get; }

        public ForeignKeyOf(Type targetEntityType)
        {
            if (targetEntityType == null)
                throw new ArgumentNullException(nameof(targetEntityType));

            if (!typeof(Entity).IsAssignableFrom(targetEntityType))
                throw new ArgumentException($"Type must inherit from {nameof(Entity)}.", nameof(targetEntityType));

            TargetEntityType = targetEntityType;
        }
    }
}
