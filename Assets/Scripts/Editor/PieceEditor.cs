#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace HexagonDenys.Editors
{
    [CustomEditor(typeof(Piece))]
    public class PieceEditor : Editor
    {
        Piece piece;

        Texture2D[] ColoredButtons;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                EditorGUILayout.HelpBox("Cannot edit while the game is playing", MessageType.Info);
            else
            {
                EditorGUILayout.HelpBox("DO NOT EDIT THE COLOR VALUE OF THE SPRITE RENDERER DIRECTLY!\nClick the Colored Buttons Below", MessageType.Warning);

                for (int i = 0; i < piece.GridPoint.Grid.PieceColors.Length; i++)
                {
                    GUIStyle guiStyle = new GUIStyle();
                    guiStyle.normal.background = ColoredButtons[i];
                    if (GUILayout.Button("", guiStyle))
                    {
                        piece.Color = piece.GridPoint.Grid.PieceColors[i];
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    }
                }
            }
        }

        private void OnEnable()
        {
            piece = (Piece)target;

            ColoredButtons = new Texture2D[piece.GridPoint.Grid.PieceColors.Length];
            for (int i = 0; i < ColoredButtons.Length; i++)
            {
                ColoredButtons[i] = new Texture2D(4, 4, TextureFormat.ARGB32, false);

                Color[] colors = new Color[16];
                for (int y = 0; y < colors.Length; y++)
                    colors[y] = piece.GridPoint.Grid.PieceColors[i];

                ColoredButtons[i].SetPixels(colors);
                ColoredButtons[i].Apply(false);
            }
        }
    }
}
#endif