using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed;

    public float Speed { get; set; }

    private Stack<Node> path;

    private Animator myAnimator;

    public Point GridPosition { get; set; }

    public bool IsActive { get; set; }

    private Vector3 destination;

    private void Update() {
        Move();
    }

    // called from Game Manager
    public void Spawn(){
        transform.position = LevelManager.Instance.BluePortal.transform.position;

        myAnimator = GetComponent<Animator>();

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));

        SetPath(LevelManager.Instance.Path);
    }


    // this makes monsters appear to grow as they are "comming through the portal"
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove){
        // IsActive = false;   // don't move while scaling

        float progress = 0; // scaling progress

        while(progress <= 1){
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;  // done just in case finished scalling is a tiny bit off like 0.98

        IsActive = true;
        if(remove){
            // Destroy(gameObject);
            Release();
        }
    } 

    private void Move(){
        if(IsActive){   // prevents movement before scaling at portal is completed
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            // destination is the next tile on the path stack
            if(transform.position == destination){
                if(path != null && path.Count > 0){
                    Animate(GridPosition, path.Peek().GridPosition);    // give current and next positions to animator
                    GridPosition = path.Peek().GridPosition;    // Peek looks but does not pop the stack
                    destination = path.Pop().WorldPosition;
                }
            }
        }        
    }

    // get start position when monster is spawned
    private void SetPath(Stack<Node> newPath){
        if(newPath != null){
            this.path = newPath;    // get path
            Animate(GridPosition, path.Peek().GridPosition);    // give current and next positions to animator
            GridPosition = path.Peek().GridPosition;    // set next position
            destination = path.Pop().WorldPosition;     // set destination to next position
        }
    }

/****** Be sure to uncheck "Has Exit Time" on animation transitions to prevent
it from automatically transitioning to the next animation. Also unckeck "Fixed Duration"
and set Transition Duration to 0. In 2D, we just want it to switch immediately to the
next direction animation ****/
    private void Animate(Point currentPos, Point newPos){
        if(currentPos.Y > newPos.Y){
            // we are moving down
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }
        else if(currentPos.Y < newPos.Y){
            // then we are moving up
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        if(currentPos.Y == newPos.Y){
            if(currentPos.X > newPos.X){
                // moving left
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if(currentPos.X < newPos.X){
                // moving right
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
    }

    // monster has reached the goal portal
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "RedPortal"){
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            other.GetComponent<Portal>().Open();
        }
    }

    // reset health and status of monster for reuse from object pool
    private void Release(){
        IsActive = false;   // reset so monster will not move until done scaling
        GridPosition = LevelManager.Instance.BlueSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);
    }
}
