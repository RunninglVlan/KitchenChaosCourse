using UnityEngine;
using UnityEngine.UIElements;

public static class ArrayExtensions {
    public static T GetRandom<T>(this T[] array) {
        return array[Random.Range(0, array.Length)];
    }
}

public static class VisualElementExtensions {
    public static void SetActive(this VisualElement element, bool value) {
        element.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

public static class StringExtensions {
    public static string ToCamel(this string value) {
        return $"{char.ToUpper(value[0])}{value[1..]}";
    }
}
