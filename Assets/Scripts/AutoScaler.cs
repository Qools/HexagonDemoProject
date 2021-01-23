using UnityEngine;
namespace HexagonDenys
{
    public class AutoScaler : MonoBehaviour
    {
        private float width, height;
        public float SIZE_ON_SCREEN;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            if (gameObject.GetComponent<Camera>())
            {
                SIZE_ON_SCREEN = Screen.height / 10;
                gameObject.GetComponent<Camera>().orthographicSize = Screen.height / SIZE_ON_SCREEN;
            }
            if (gameObject.GetComponent<Piece>())
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / SIZE_ON_SCREEN, Screen.height / SIZE_ON_SCREEN);
            }
        }


        private void Update()
        {
            //For Testing
            CheckResolution();

        }

        void CheckResolution()
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            if (gameObject.GetComponent<Camera>())
            {
                SIZE_ON_SCREEN = Screen.height / 10;
                gameObject.GetComponent<Camera>().orthographicSize = Screen.height / SIZE_ON_SCREEN;
            }
            if (gameObject.GetComponent<Piece>())
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / SIZE_ON_SCREEN, Screen.height / SIZE_ON_SCREEN);
            }
        }
    }

}
