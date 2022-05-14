using System.Collections.Generic;
using UnityEngine;

namespace Clover
{
    public static partial class Helper
    {
        public static void SetActive(Transform tras, bool active)
        {
            if (tras == null)
            {
                return;
            }

            tras.gameObject.SetActive(active);
        }

        public static void SetActive(GameObject go, bool active)
        {
            if (go == null)
            {
                return;
            }

            go.SetActive(active);
        }

        public static void SetLayerRecursively(Transform parent, int layer, int ignoreMask = 0)
        {
            if (parent == null)
            {
                return;
            }

            List<Transform> temp = TempList<Transform>.Value;
            parent.GetComponentsInChildren(true, temp);
            for (int i = 0; i < temp.Count; i++)
            {
                Transform transform = temp[i];
                if (((1 << transform.gameObject.layer) & ignoreMask) == 0)
                {
                    transform.gameObject.layer = layer;
                }
            }
        }

        public static Transform CreateChild(this GameObject go, string name)
        {
            GameObject child = new GameObject();
            child.transform.SetParent(go.transform);
            if (string.IsNullOrEmpty(name))
            {
                child.name = "InvalidName";
                return child.transform;
            }

            child.name = name;
            return child.transform;
        }

        public static Transform CreateChild(this Transform trans, string name)
        {
            GameObject child = new GameObject();
            child.transform.SetParent(trans);
            if (string.IsNullOrEmpty(name))
            {
                child.name = "InvalidName";
                return child.transform;
            }

            child.name = name;
            return child.transform;
        }

        public static void StretchHorizontalAndVerticle(this RectTransform transform)
        {
            transform.anchorMax = Vector2.one;
            transform.anchorMin = Vector2.zero;
        }

        public static void StretchTop(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 1);
            transform.anchorMin = new Vector2(1, 1);
        }

        public static void StretchHorizontal(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(1, 0.5f);
            transform.anchorMin = new Vector2(0, 0.5f);
        }

        public static void StretchBottom(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 0);
            transform.anchorMin = new Vector2(1, 0);
        }

        public static void StretchRight(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(1, 0);
            transform.anchorMin = new Vector2(1, 1);
        }

        public static void StretchVerticle(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0.5f, 1);
            transform.anchorMin = new Vector2(0.5f, 0);
        }

        public static void StretchLeft(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 1);
            transform.anchorMin = new Vector2(0, 0);
        }

        public static void AnchorLeftTop(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 1);
            transform.anchorMin = new Vector2(0, 1);
        }

        public static void AnchorTop(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0.5f, 1);
            transform.anchorMin = new Vector2(0.5f, 1);
        }

        public static void AnchorRightTop(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(1, 1);
            transform.anchorMin = new Vector2(1, 1);
        }

        public static void AnchorLeft(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 0.5f);
            transform.anchorMin = new Vector2(0, 0.5f);
        }

        public static void AnchorCenter(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0.5f, 0.5f);
            transform.anchorMin = new Vector2(0.5f, 0.5f);
        }

        public static void AnchorRight(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(1, 0.5f);
            transform.anchorMin = new Vector2(1, 0.5f);
        }

        public static void AnchorLeftBottom(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0, 0);
            transform.anchorMin = new Vector2(0, 0);
        }

        public static void AnchorBottom(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(0.5f, 0);
            transform.anchorMin = new Vector2(0.5f, 0);
        }

        public static void AnchorRightBottom(this RectTransform transform)
        {
            transform.anchorMax = new Vector2(1, 0);
            transform.anchorMin = new Vector2(1, 0);
        }
    }
}