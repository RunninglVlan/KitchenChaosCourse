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
