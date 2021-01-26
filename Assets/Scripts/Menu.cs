using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HexagonDenys
{
    public class Menu : MonoBehaviour
    {
        public static Menu Instance;

        public Text TextScore;
        public Text TextNumMoves;

        private int score = 0;
        public int Score
        {
            get => score;
            set => TextScore.text = "Score \n" + (score = value).ToString();
        }
        private int numMoves = 0;
        public int NumMoves
        {
            get => numMoves;
            set
            {
                TextNumMoves.text = "# of Moves \n" + (numMoves = value).ToString();
                Bomb.TickAllBombs();
            }
        }

        private void Awake() => Menu.Instance = this;

        public void Restart()
        {
            foreach (Bomb bomb in Bomb.All)
                Destroy(bomb.gameObject);
            Bomb.All.Clear();

            Piece.Unused.Clear();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}