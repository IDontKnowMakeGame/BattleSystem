public class ConverterFormat
{
    public static string classFormat =
        @"public class {0}Data
{{
    {1}
}}";
    public static string variableFormat =
        @"public static readonly {0};";

    public static string enumFormat =
        @"enum {0}
{{
    Empty,
    {1}End
}}";
}
