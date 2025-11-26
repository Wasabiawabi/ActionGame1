using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("スペースキーで上に移動.\nAキーでその場に止まる.")]
    //アタッチ
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private CapsuleCollider2D groundTouchTrigger;

    //使う変数
    [SerializeField] private bool summary;
    public bool movingToUpward = false;
    public bool movingToDownward = false;
    public bool stopped = false;
    public bool started = false;
    public bool isTouchingGround = false;

    public void InitializeController()
    {
        movingToUpward = false;
        stopped = false;
        started = false;
    }
    private void Update()
    {
        //Enterが押されたらスタート
        if(Input.GetKeyDown(KeyCode.Return))
        {
            started = true;
            playerMovementHandler.GameStart();
        }

        //Wキーが押されている間、上に移動し続ける
        if(Input.GetKeyDown(KeyCode.W))
        {
            movingToUpward = true;
            isTouchingGround = false;
        }

        //Wキーが離されたら、上への移動を止める
        if(Input.GetKeyUp(KeyCode.W))
        {
            movingToUpward = false;
        }

        //Sキーが押されている間、下に移動し続ける
        if(Input.GetKeyDown(KeyCode.S))
        {
            movingToDownward = true;
        }

        //Sキーが離されたら、下への移動を止める
        if(Input.GetKeyUp(KeyCode.S))
        {
            movingToDownward = false;
        }

        //Aボタンが押されている間は、その場に止まる
        if(Input.GetKeyDown(KeyCode.A))
        {
            stopped = true;
        }

        //Aボタンが離されたら、再び移動を開始する
        if(Input.GetKeyUp(KeyCode.A))
        {
            stopped = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTouchingGround = true;
       //Debug.Log("touch");
    }
}
