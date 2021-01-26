#define DEBUG
#if UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
namespace HexagonDenys
{
    public class AutoScaler : MonoBehaviour
    {
        private float width, height;
        public float SIZE_ON_SCREEN;
        public bool Enable = false;
        [SerializeField] private string UITag, TextTag, GridTag;
        private RectTransform rect;

        // Start is called before the first frame update
        private void Start()
        {
            rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Screen.width, Screen.height);
            if (gameObject.GetComponent<Camera>())
            {
                SIZE_ON_SCREEN = 2;
                gameObject.GetComponent<Camera>().orthographicSize = Screen.height / SIZE_ON_SCREEN;
                switch (Screen.width)
                {
                    case 720:
                        rect.anchorMin = new Vector2(1, 0.9f);
                        rect.anchorMax = new Vector2(1, 0.9f);
                        break;
                    case 1080:
                        rect.anchorMin = new Vector2(0.925f, 0.9f);
                        rect.anchorMax = new Vector2(0.925f, 0.9f);
                        break;
                    case 1440:
                        rect.anchorMin = new Vector2(0.8f, 0.9f);
                        rect.anchorMax = new Vector2(0.8f, 0.9f);
                        break;
                }
                
            }
            if (Enable)
            {
                if (gameObject.tag == UITag)
                {
                    SIZE_ON_SCREEN = 10;
                    rect.sizeDelta = new Vector2(Screen.width / SIZE_ON_SCREEN, Screen.height / SIZE_ON_SCREEN);
                    switch (Screen.height)
                    {
                        case 2220:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2160:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2960:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2560:
                            SIZE_ON_SCREEN = 1.5f;
                            break;
                        case 1920:
                            SIZE_ON_SCREEN = 1.5f;
                            break;
                    }
                    
                    rect.position = new Vector2(rect.position.x, Screen.width * SIZE_ON_SCREEN);
                }
                if (gameObject.tag == TextTag)
                {
                    SIZE_ON_SCREEN = 10;
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, Screen.height / SIZE_ON_SCREEN);
                    
                    if (gameObject.GetComponent<Text>())
                    {
                        gameObject.GetComponent<Text>().fontSize = (int)(Screen.height / (SIZE_ON_SCREEN * 4f));
                    }
                }
            }
        }

#if UNITY_EDITOR && DEBUG
        private void Update() => CheckResolution();
#endif
        private void CheckResolution()
        {
            rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Screen.width, Screen.height);
            if (gameObject.GetComponent<Camera>())
            {
                SIZE_ON_SCREEN = 2;
                gameObject.GetComponent<Camera>().orthographicSize = Screen.height / SIZE_ON_SCREEN;

                switch (Screen.width)
                {
                    case 720:
                        rect.anchorMin = new Vector2(1, 0.9f);
                        rect.anchorMax = new Vector2(1, 0.9f);
                        break;
                    case 1080:
                        rect.anchorMin = new Vector2(0.9f, 0.9f);
                        rect.anchorMax = new Vector2(0.9f, 0.9f);
                        break;
                    case 1440:
                        rect.anchorMin = new Vector2(0.8f, 0.9f);
                        rect.anchorMax = new Vector2(0.8f, 0.9f);
                        break;
                }
            }
            if (Enable)
            {
                if (gameObject.tag == UITag)
                {
                    SIZE_ON_SCREEN = 10;
                    rect.sizeDelta = new Vector2(Screen.width / SIZE_ON_SCREEN, Screen.height / SIZE_ON_SCREEN);
                    switch (Screen.height)
                    {
                        case 2220:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2160:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2960:
                            SIZE_ON_SCREEN = 1.75f;
                            break;
                        case 2560:
                            SIZE_ON_SCREEN = 1.5f;
                            break;
                        case 1920:
                            SIZE_ON_SCREEN = 1.5f;
                            break;
                    }
                    rect.position = new Vector2(rect.position.x, Screen.width * SIZE_ON_SCREEN);
                }

                if (gameObject.tag == TextTag)
                {
                    SIZE_ON_SCREEN = 10;
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, Screen.height / SIZE_ON_SCREEN);
                    if (gameObject.GetComponent<Text>())
                    {
                        gameObject.GetComponent<Text>().fontSize = (int)(Screen.height / (SIZE_ON_SCREEN * 4f));
                    }
                }
            }
        }
    }
}
#endif