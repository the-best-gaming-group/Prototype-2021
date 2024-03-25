using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzle : MonoBehaviour
{
    [SerializeField] private Transform empty;
    [SerializeField] private GameObject winUI;
    [SerializeField] private Transform Tile1;
    [SerializeField] private Transform Tile2;
    [SerializeField] private Transform Tile3;
    [SerializeField] private Transform Tile4;
    [SerializeField] private Transform Tile5;
    [SerializeField] private Transform Tile6;
    [SerializeField] private Transform Tile7;
    [SerializeField] private Transform Tile8;
    private Camera cam;
    private AudioSource audioSource;
    public AudioClip puzzleComplete;
    private Vector3 T1;
    private Vector3 T2;
    private Vector3 T3;
    private Vector3 T4;
    private Vector3 T5;
    private Vector3 T6;
    private Vector3 T7;
    private Vector3 T8;
    private bool Won = false;
    float winCheck;
    void Start()
    {
        cam = Camera.main;
        T1.Set(-20,50,-1);
        T2.Set(30,50,-1);
        T3.Set(80,50,-1);
        T4.Set(-20,0,-1);
        T5.Set(30,0,-1);
        T6.Set(80,0,-1);
        T7.Set(-20,-50,-1);
        T8.Set(30,-50,-1);
        winCheck = 0f;
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        winCheck += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            Ray pointer = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pointer.origin, pointer.direction);
            if (hit && !Won)
            {
                if (Vector2.Distance(empty.position,hit.transform.position) < 60)
                {
                    audioSource.Stop();
                    audioSource.Play();
                    Vector3 lastEmptySpace = empty.position;
                    TileMovement thisTile = hit.transform.GetComponent<TileMovement>();
                    empty.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpace;
                    
                }
            }
        }
        while (winCheck >= 2)
        {
            CheckFinished();
            winCheck -= 2;
        }
    }

    void CheckFinished()
    {
        if (ComparePositions(Tile1.position, T1) && ComparePositions(Tile2.position, T2) && ComparePositions(Tile3.position, T3) && ComparePositions(Tile4.position, T4) && ComparePositions(Tile5.position, T5) && ComparePositions(Tile6.position, T6) && ComparePositions(Tile7.position, T7) && ComparePositions(Tile8.position, T8))
        {
            //audioSource.Stop();
            //audioSource.PlayOneShot(puzzleComplete);
            Debug.Log("Win");
            winUI.SetActive(true);  
            Won = true;
        }
    }

    bool ComparePositions(Vector3 A, Vector3 B)
    {
        if (Mathf.Round(A.x) == B.x && Mathf.Round(A.y) == B.y)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
