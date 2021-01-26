using System.Collections.Generic;
using UnityEngine;

namespace HexagonDenys
{
    public class Bomb : Piece
    {
        public static List<Bomb> All = new List<Bomb>();

        public TextMesh TextMesh;
        public string Text
        {
            get => TextMesh.text;
            set => TextMesh.text = value;
        }
        [System.NonSerialized]
        public int CreatedAtMove;
        [System.NonSerialized]
        public int Countdown;
        public int RemainingMoves => Countdown - (Menu.Instance.NumMoves - CreatedAtMove);

        private static bool exploded;
        public static bool Exploded
        {
            get
            {
                bool returnVal = exploded;
                exploded = false;
                return returnVal;
            }
            set => exploded = value;
        }

        public static Bomb CreateNew(GridPoint gridPoint)
        {
            Bomb bomb = Piece.CreateNewSprite().AddComponent<Bomb>();
            bomb.gameObject.name = "bomb";

            bomb.transform.localScale = new Vector3(CustomGrid.Instance.PieceScale.x, CustomGrid.Instance.PieceScale.x);
            bomb.gameObject.SetActive(true);
            Bomb.All.Add(bomb);

            bomb.image.sprite = CustomGrid.Instance.BombSprite;

            GameObject temp = new GameObject("textMesh");
            RectTransform rect = temp.AddComponent<RectTransform>();
            rect.parent = bomb.transform;
            bomb.TextMesh = temp.AddComponent<TextMesh>();
            bomb.TextMesh.alignment = TextAlignment.Right;
            bomb.TextMesh.anchor = TextAnchor.MiddleRight;
            bomb.TextMesh.color = Color.black;
            bomb.TextMesh.characterSize = Screen.height / 50f;
            bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().sortingOrder = 9;

            gridPoint.Piece = bomb;
            bomb.Color = CustomGrid.Instance.PieceColors[Random.Range(0, CustomGrid.Instance.PieceColors.Length)];
            bomb.TimeActivated = Time.time;
            bomb.transform.localPosition = bomb.GridPosStart;
            bomb.CreatedAtMove = Menu.Instance.NumMoves;
            bomb.Countdown = Random.Range(7, 10);
            bomb.Tick(); //Do this once to display text.

            return bomb;
        }

        public static void TickAllBombs()
        {
            foreach (Bomb bomb in Bomb.All)
            {
                if (bomb == null)
                {
                    Bomb.All.Remove(bomb);
                    continue;
                }

                bomb.Tick();
            }
        }

        private void Tick()
        {
            TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = true;
            TextMesh.text = RemainingMoves.ToString();
        }

        public static void CheckFuses()
        {
            foreach (Bomb bomb in Bomb.All)
            {
                if (bomb == null)
                {
                    Bomb.All.Remove(bomb);
                    continue;
                }

                if (bomb.RemainingMoves <= 0)
                {
                    Menu.Instance.Restart();
                    Bomb.Exploded = true;
                    return;
                }
            }
        }

    }
}