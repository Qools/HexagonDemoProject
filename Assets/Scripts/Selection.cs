using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonDenys
{
    public class Selection : MonoBehaviour
    {
        public GameObject ForegroundObject;
        public GameObject Piece0;
        public GameObject Piece1;
        public GameObject Piece2;

        private Image Piece0SpriteRenderer;
        public Color Piece0Color
        {
            get => Piece0SpriteRenderer.color;
            set => Piece0SpriteRenderer.color = value;
        }

        private Image Piece1SpriteRenderer;
        public Color Piece1Color
        {
            get => Piece1SpriteRenderer.color;
            set => Piece1SpriteRenderer.color = value;
        }

        private Image Piece2SpriteRenderer;
        public Color Piece2Color
        {
            get => Piece2SpriteRenderer.color;
            set => Piece2SpriteRenderer.color = value;
        }

        private Image Bomb0SpriteRenderer;
        private Image Bomb1SpriteRenderer;
        private Image Bomb2SpriteRenderer;

        [System.NonSerialized]
        public GridJunction SelectedGridJunction;

        private void Start()
        {
            CustomGrid.Instance.Selection = this;

            Piece0SpriteRenderer = Piece0.GetComponent<Image>();
            Piece1SpriteRenderer = Piece1.GetComponent<Image>();
            Piece2SpriteRenderer = Piece2.GetComponent<Image>();

            Bomb0SpriteRenderer = Piece0.transform.GetChild(0).gameObject.GetComponent<Image>();
            Bomb1SpriteRenderer = Piece1.transform.GetChild(0).gameObject.GetComponent<Image>();
            Bomb2SpriteRenderer = Piece2.transform.GetChild(0).gameObject.GetComponent<Image>();

            
            ForegroundObject.transform.localScale = Vector3.one * CustomGrid.Instance.PieceScale.x / 4;
            transform.localScale = CustomGrid.Instance.PieceScale;

            Deactivate();
            ForegroundObject.transform.parent = null;
        }

        public void Reactivate()
        {
            Activate(Camera.allCameras[0].WorldToScreenPoint(transform.position));
        }
        public void Activate(Vector2 screenPoint)
        {
            Activate(new Vector3(screenPoint.x, screenPoint.y, 0));
        }
        public void Activate(Vector3 screenPoint)
        {
            if (CustomGrid.Instance == null)
                return;

            //Play audio
            CustomGrid.Instance.AudioSource.PlayOneShot(CustomGrid.Instance.AC_PieceSelect);

            Vector3 worldPoint = Camera.allCameras[0].ScreenToWorldPoint(screenPoint);

            //Find the closest GridJunction
            GridJunction closest = null;
            float closestDist = float.MaxValue;
            for (int x = 0; x < CustomGrid.Instance.GridJunctions.GetLength(0); x++)
            {
                for (int y = 0; y < CustomGrid.Instance.GridJunctions.GetLength(1); y++)
                {
                    float dist = Vector3.Distance(CustomGrid.Instance.GridJunctions[x, y].WorldPosition, worldPoint);

                    if (dist > closestDist)
                        continue;

                    closest = CustomGrid.Instance.GridJunctions[x, y];
                    closestDist = dist;
                }
            }

            SelectedGridJunction = closest;

            //Set gameobject active
            gameObject.SetActive(true);
            ForegroundObject.SetActive(true);

            //Disable bombs and enable hexagons
            Bomb0SpriteRenderer.enabled = Bomb1SpriteRenderer.enabled = Bomb2SpriteRenderer.enabled = false;
            Piece0SpriteRenderer.enabled = Piece1SpriteRenderer.enabled = Piece2SpriteRenderer.enabled = true;

            //Set position to GridJunction
            ForegroundObject.transform.position = transform.position = SelectedGridJunction.WorldPosition;

            //Rotate Bg and reposition pieces based on oddness
            float LenghtX = 39f;
            float LenghtY = 49f;
            Piece0.transform.localPosition = SelectedGridJunction.IsOdd ? new Vector3(-LenghtX, 0) : new Vector3(LenghtX, 0);
            Piece1.transform.localPosition = SelectedGridJunction.IsOdd ? new Vector3(LenghtX, -LenghtY) : new Vector3(-LenghtX, -LenghtY);
            Piece2.transform.localPosition = SelectedGridJunction.IsOdd ? new Vector3(LenghtX, LenghtY) : new Vector3(-LenghtX, LenghtY);

            //Assign Colors
            Piece0Color = SelectedGridJunction.GridPoints[0].Piece.Color;
            Piece1Color = SelectedGridJunction.GridPoints[1].Piece.Color;
            Piece2Color = SelectedGridJunction.GridPoints[2].Piece.Color;

            //Check if any of the pieces are actually bombs
            if (SelectedGridJunction.GridPoints[0].Piece is Bomb)
            {
                Piece0SpriteRenderer.enabled = false;
                Bomb0SpriteRenderer.enabled = true;
                Bomb0SpriteRenderer.color = Piece0SpriteRenderer.color;
            }
            if (SelectedGridJunction.GridPoints[1].Piece is Bomb)
            {
                Piece1SpriteRenderer.enabled = false;
                Bomb1SpriteRenderer.enabled = true;
                Bomb1SpriteRenderer.color = Piece1SpriteRenderer.color;
            }
            if (SelectedGridJunction.GridPoints[2].Piece is Bomb)
            {
                Piece2SpriteRenderer.enabled = false;
                Bomb2SpriteRenderer.enabled = true;
                Bomb2SpriteRenderer.color = Piece2SpriteRenderer.color;
            }
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            ForegroundObject.SetActive(false);
        }

        private IEnumerator RotateEnumerator;
        public void RotateCounterClockwise() => StartCoroutine(RotateEnumerator = Rotate(1f));
        public void RotateClockwise() => StartCoroutine(RotateEnumerator = Rotate(-1f));

        private IEnumerator Rotate(float dir)
        {
            //Play Audio
            if (dir < 0)
                CustomGrid.Instance.AudioSource.PlayOneShot(CustomGrid.Instance.AC_PieceClockwise);
            else
                CustomGrid.Instance.AudioSource.PlayOneShot(CustomGrid.Instance.AC_PieceCounterClockwise);

            //Hide TextMesh if bomb. These will get reactivated after the move automaticly
            if (SelectedGridJunction.GridPoints[0].Piece is Bomb)
            {
                Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[0].Piece;
                bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if (SelectedGridJunction.GridPoints[1].Piece is Bomb)
            {
                Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[1].Piece;
                bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if (SelectedGridJunction.GridPoints[2].Piece is Bomb)
            {
                Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[2].Piece;
                bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            //Set GameReady state to false so player cannot make any more moves until the current one is finished
            CustomGrid.Instance.GameReady = false;

            //Divide desired rotationTime by 3 because the rotation happens in 3 equal stages.
            //Doing this will enable the use of Lerp method.
            float rotationTime = 1.0f / 3;

            for (int i = 0; i < 3; i++)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, dir * 120 * (i + 1));
                Quaternion startRotation = Quaternion.Euler(0, 0, dir * 120f * i);
                float startTime = Time.time;
                while (Time.time < startTime + rotationTime)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, rotation, (1f / rotationTime) * (Time.time - startTime));
                    yield return null;
                }
                transform.rotation = rotation;

                //Todo: Actually switch pieces here
                if (dir > 0)
                    SelectedGridJunction.SwitchPiecesClockwise();
                else
                    SelectedGridJunction.SwitchPiecesCounterClockwise();

                if (CustomGrid.Instance.ExplosionOccurred = CustomGrid.Instance.CheckForExplosion(SelectedGridJunction))
                {
                    transform.rotation = Quaternion.identity;
                    Deactivate();
                    break;
                }

                yield return null;
            }

            //Increase NumMoves if explosion occurred otherwise Set GameReady state back to true so player can make new moves.
            if (CustomGrid.Instance.ExplosionOccurred)
            {
                Menu.Instance.NumMoves++;
            }

            else
            {
                CustomGrid.Instance.GameReady = true;

                //enable TextMeshRenderers back
                if (SelectedGridJunction.GridPoints[0].Piece is Bomb)
                {
                    Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[0].Piece;
                    bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (SelectedGridJunction.GridPoints[1].Piece is Bomb)
                {
                    Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[1].Piece;
                    bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (SelectedGridJunction.GridPoints[2].Piece is Bomb)
                {
                    Bomb bomb = (Bomb)SelectedGridJunction.GridPoints[2].Piece;
                    bomb.TextMesh.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}