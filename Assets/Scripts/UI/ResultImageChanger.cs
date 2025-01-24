using UnityEngine;
using UnityEngine.UI;
public class ResultImageChanger : MonoBehaviour
{
    [SerializeField] public Image resultImage;  // 対象のImageコンポーネント
    [SerializeField] public Sprite winImage1P; // 1P勝利のSprite
    [SerializeField] public Sprite winImage2P; // 2P勝利のSprite

    public void ChangeResultImage(int winner)
    {
        if (resultImage == null)
        {
            Debug.LogError("Result Image is not assigned!");
            return;
        }

        // 勝者に応じてSource Imageを変更
        if (winner == 1)
        {
            resultImage.sprite = winImage1P; // 1Pの勝利画像に変更
        }
        else if (winner == 2)
        {
            resultImage.sprite = winImage2P; // 2Pの勝利画像に変更
        }
        else
        {
            Debug.LogWarning("Invalid winner value!");
        }
    }

    private void Start()
    {
        if(GoalChecker.Instance)
            ChangeResultImage(GoalChecker.Instance.GetWinner());
    }
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeResultImage(2);
        }
    }

}
