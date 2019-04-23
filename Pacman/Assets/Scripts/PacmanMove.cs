using UnityEngine;

public class PacmanMove : MonoBehaviour
{   //移动速度
    public float speed = 0.35f;
    //初始化位置
    Vector2 dest = Vector2.zero;

    

    private void Start()
    {   //进入游戏后的初始位置
        dest = transform.position;

    }

    private void FixedUpdate()
    {
        //插值得到要移动到dest位置的下一次坐标
        Vector2 newposition = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体组件使物体移动到新位置
        GetComponent<Rigidbody2D>().MovePosition(newposition);
        //物体到达dest位置时再更改dest
        if ((Vector2)transform.position == dest)
        {


            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            
        }
        Vector2 dis = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DieX", dis.x);
        GetComponent<Animator>().SetFloat("DieY", dis.y);






    }


    //检测将要去的位置是否可到达
    private bool Valid(Vector2 step)
    {   //物体当前位置
        Vector2 pos = transform.position;
        //从要到达位置向当前位置引一条射线
        RaycastHit2D hit = Physics2D.Linecast(pos + step, pos);
        //返回射线是否打到物体本身的碰撞器
        return (hit.collider == GetComponent<Collider2D>());
    }


}
