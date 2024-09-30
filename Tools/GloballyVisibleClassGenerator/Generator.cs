using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using System.Reflection;

internal class Generator
{
    internal static void Generate(string[] args, ILogger logger)
    {
        string modelsDllPath = args[0];
        string generatedModelsPath = args[1];
        string generationParamsString = "";

        if (args.Length > 2)
        {
            generationParamsString = args[2];
        }

        var generationParams = JsonConvert.DeserializeObject<GloballyVisibleClassGeneratorParams>(generationParamsString);

        string namespaceName = generationParams?.Namespace ?? "Generated";

        if (!File.Exists(modelsDllPath))
        {
            logger.LogError($"DLL file not found: {modelsDllPath}");
            return;
        }

        if (Directory.GetParent(generatedModelsPath)?.Exists != true)
        {
            logger.LogError($"Parent directory ({Directory.GetParent(generatedModelsPath)?.Parent?.FullName ?? ""}) does not exist!");
            return;
        }

        if (Directory.Exists(generatedModelsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(generatedModelsPath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
                logger.LogInformation($"{file.FullName} removed");
            }
        }
        else
        {
            Directory.CreateDirectory(generatedModelsPath);
            logger.LogInformation($"Creted {generatedModelsPath}");
        }

        Assembly modelsAssembly;
        try
        {
            modelsAssembly = Assembly.LoadFrom(modelsDllPath);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error loading assembly: {ex.Message}");
            return;
        }

        var classesToGenerate = modelsAssembly.GetTypes()
            .Where(t => t.GetCustomAttributes(false)
            .Any(a => a.GetType() == typeof(GloballyVisibleAttribute))).ToList();

        if (generationParams?.CopyBaseClasses == true)
        {
            classesToGenerate.ToList().ForEach((type) =>
            {
                var baseClassTree = GetDependenceTree(type);

                baseClassTree.ForEach(baseType =>
                {
                    if (!classesToGenerate.Contains(baseType))
                    {
                        classesToGenerate.Add(baseType);
                    }
                });
            });
        }

        List<string> alreadyGenerated = [];

        foreach (var type in classesToGenerate)
        {
            GenerateClassCopy(type, generatedModelsPath, namespaceName, generationParams, logger, alreadyGenerated.Count(x => x == type.Name));
            alreadyGenerated.Add(type.Name);
        }

        logger.LogInformation("Generation complete.");
    }

    public static List<Type> GetDependenceTree(Type type)
    {
        var dependencies = new HashSet<Type>(); // Using HashSet to avoid duplicates

        // Collect dependencies recursively for the given type
        CollectTypeDependencies(type, dependencies);

        return dependencies.ToList();
    }

    private static void CollectTypeDependencies(Type type, HashSet<Type> dependencies)
    {
        // Return if type is null, already processed, or a system type
        if (type == null || type == typeof(object) || dependencies.Contains(type) || IsSystemType(type))
            return;

        // Add the current user-defined type to dependencies
        dependencies.Add(type);

        // Collect base classes recursively
        var baseType = type.BaseType;
        if (baseType != null && baseType != typeof(object))
        {
            CollectTypeDependencies(baseType, dependencies);
        }

        // Collect property types recursively
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            var propType = prop.PropertyType;

            // Handle generic properties (e.g., ICollection<T>)
            if (propType.IsGenericType)
            {
                foreach (var genericArg in propType.GetGenericArguments())
                {
                    CollectTypeDependencies(genericArg, dependencies);
                }
            }

            // Recursively collect dependencies for the property type
            CollectTypeDependencies(propType, dependencies);
        }
    }

    // Check if the type is a system or framework type
    private static bool IsSystemType(Type type)
    {
        // Filter system types by namespace or assembly
        return type.Namespace != null &&
               (type.Namespace.StartsWith("System") || type.Namespace.StartsWith("Microsoft") ||
                type.Assembly.FullName.StartsWith("System") || type.Assembly.FullName.StartsWith("Microsoft"));
    }
    static void GenerateClassCopy(Type type, string outputDirectory, string namespaceName, IGloballyVisibleClassGeneratorParams? generationParams, ILogger logger, int countOfSameClassNames)
    {
        string className = type.Name;
        string newClassName = className;

        string outputPath = Path.Combine(outputDirectory, newClassName.Split('`')[0] + $"{(countOfSameClassNames > 0
            ? "_" + countOfSameClassNames.ToString()
            : "")}.cs");

        using (StreamWriter writer = new StreamWriter(outputPath, false))
        {
            // Get the base class type
            var baseClassType = type.BaseType;

            WriteClassPrefix(namespaceName, type, baseClassType, writer);
            WriteBaseClass(baseClassType, writer);
            WriteMethods(type, writer);
            WriteClassSuffix(writer);
        }
    }
    private static void WriteClassSuffix(StreamWriter writer)
    {
        writer.WriteLine("\t}");
        writer.WriteLine("}");
    }
    private static void WriteMethods(Type type, StreamWriter writer)
    {
        // Get properties declared only in this class (not inherited from base classes)
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.DeclaringType == type); // Exclude base class properties

        foreach (var prop in properties)
        {
            var isHidden = prop.GetCustomAttributes(false)
                .Any(a => a.GetType() == typeof(GloballyHiddenAttribute));

            if (isHidden)
                continue;

            // Handle generic types
            var propertyType = prop.PropertyType;
            string typeName;

            if (propertyType.IsGenericType)
            {
                var genericTypeDefinition = propertyType.GetGenericTypeDefinition();
                var genericTypeName = genericTypeDefinition.Name.Split('`')[0]; // Strip off `1
                var genericArguments = string.Join(", ", propertyType.GetGenericArguments().Select(t => t.Name));

                typeName = $"{genericTypeName}<{genericArguments}>";
            }
            else
            {
                typeName = propertyType.Name;
            }

            // Write the property to the output
            writer.WriteLine($"\t\tpublic {typeName} {prop.Name} {{ get; set; }}");
        }
    }
    private static void WriteClassPrefix(string namespaceName, Type newClass, Type? baseClassType, StreamWriter writer)
    {
        writer.WriteLine($"namespace {namespaceName}");
        writer.WriteLine("{");

        writer.WriteLine($"\t// Generated Code\n");

        // Handle generics in the class name (if any)
        if (newClass.IsGenericType)
        {
            var genericArguments = newClass.GetGenericArguments();
            var genericTypeNames = string.Join(", ", genericArguments.Select(arg => arg.Name));

            // Write class name with generic parameters
            writer.Write($"\tpublic class {newClass.Name.Split('`')[0]}<{genericTypeNames}>");
        }
        else
        {
            // No generic parameters in the class name
            writer.Write($"\tpublic class {newClass.Name}");
        }
    }
    private static void WriteBaseClass(Type baseClassType, StreamWriter writer)
    {
        if (baseClassType != null)
        {
            if (baseClassType.IsGenericType)
            {
                var genericArguments = baseClassType.GetGenericArguments();
                var genericTypeNames = string.Join(", ", genericArguments.Select(arg => arg.Name));

                // Write base class with its generic parameters
                writer.Write($" : {baseClassType.Name.Split('`')[0]}<{genericTypeNames}>");
            }
            else
            {
                // Write non-generic base class
                writer.Write($" : {baseClassType.Name}");
            }
        }

        writer.WriteLine("\t{"); // Open the body of the class
    }
}


//inheritedebi unda davikidot
//unda gadavitanot nullablebi
//genericebi
//collectionebi
//base classebi
