using Logic.Shapes;
using System.Reflection;

namespace HillClimbing_ImageRecreation.Data
{
    public class ShapeTypesProvider
    {
        public class ShapeTypeWithDisplayName
        {
            public Type ShapeType { get; set; }
            public string DisplayName { get; set; }
        }

        public List<ShapeTypeWithDisplayName> ShapeTypesWithDisplayNames { get; init; }
        public ShapeTypesProvider()
        {
            ShapeTypesWithDisplayNames = new List<ShapeTypeWithDisplayName>();
            var shapeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Shape).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            foreach (var shapeType in shapeTypes)
            {
                var displayNameProperty = shapeType.GetProperty("DisplayName", BindingFlags.Static | BindingFlags.Public);
                ShapeTypesWithDisplayNames.Add(new()
                {
                    ShapeType = shapeType,
                    DisplayName = (string)displayNameProperty.GetValue(null)
                });
            }
        }
    }
}
