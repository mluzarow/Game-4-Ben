using UnityEngine;
using System.Collections;

public class BossAI : bossRaycastController{
    public Rect HealthBar = new Rect (Screen.width / 4, 10f, Screen.width / 2, 20f);
    public Texture2D HealthBarTexture;
    public float bossHP = 3000;
    public GameObject bullet;
    public Vector2 shootSpeed = new Vector2 (5f, 5f);
    public float moveSpeed = 7f;
    public LayerMask playerBulletMask;

    private float bossBaseHP = 3000;
    private float healthBaseWidth = Screen.width / 2;
    private bool fired = false;
    private float diffx;
    private float diffy;
    private float diff = 10000;

    public GameObject player;
	
	// Update is called once per frame
	void Update () {
        transform.Translate (moveSpeed * Time.deltaTime, 0f, 0f);
        updateRaycasts();

        verticalCollisions(moveSpeed);


        if (transform.position.x >= 230f) {
            moveSpeed *= -1;
        } else if (transform.position.x <= 195f) {
            moveSpeed *= -1;
        }

        if (bossHP <= 0) {
            Application.LoadLevel ("stage4EndGame");
        }

        Vector2 playerPos = player.transform.position;
        Vector2 bossPos = transform.position;

        float diffx = playerPos.x - bossPos.x;
        float diffy = playerPos.y - bossPos.y;

        Vector2 delta = new Vector2 (diffx, diffy);
        float deltaMag = Mathf.Sqrt(Mathf.Pow(diffx, 2) + Mathf.Pow(diffy, 2));
        Vector2 deltaHat = new Vector2 (diffx / deltaMag, diffy / deltaMag);

        Vector2 newVelocity = new Vector2 (deltaHat.x * shootSpeed.x, deltaHat.y * shootSpeed.y);

        HealthBar = new Rect (Screen.width / 4, 10f, healthBaseWidth * (bossHP / bossBaseHP), 20f);
        Debug.Log(HealthBar);

        if (deltaMag <= 50f && fired == false) {
            GameObject clone = (GameObject) Instantiate (bullet, transform.position, Quaternion.identity);
            clone.transform.GetComponent<spawnerClone> ().setVelocity(newVelocity);

            StartCoroutine(dashingTimer());
        }
    }

    //velocity in the x plane is NOT 0; check for collisions
    void horizontalCollisions (float speed) {
        float directionX = (speed >= 0) ? 1 : -1;
        float rayLength = Mathf.Abs(1) + inset;

        //Extend rays to detect walls while not moving towards them
        if (Mathf.Abs(speed) < inset) {
            rayLength = 2 * inset;
        }

        for (int i = 0; i < horizontalRays; i++) {
            //If directionX is negative (moving left), raycast from the bottom left
            //If directionX is positive (moving right), raycast from the bottom right
            Vector2 rayOrigin = (directionX == -1) ? raycasts.bottomLeft : raycasts.bottomRight;
            //Add horizontal ray spacing to raycast position
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hurt = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, playerBulletMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hurt) {
                
                bossHP -= 5;
                print(bossHP);
            }
        }
    }

    void verticalCollisions (float speed) {
        float rayLength = Mathf.Abs(1) + inset;

        for (int i = 0; i < verticalRays; i++) {
            Vector2 rayOrigin = raycasts.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + 1);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * -1, rayLength, playerBulletMask);

            Debug.DrawRay(rayOrigin, Vector2.up * -1 * rayLength, Color.red);

            if (hit) {
                bossHP -= 5;
                print(bossHP);
            }
        }
    }

    IEnumerator dashingTimer () {
        fired = true;
        //dashTiming = true;
        yield return new WaitForSeconds(1f);
        fired = false;
        yield return 0;
    }

    void OnGUI () {
        GUI.DrawTexture(HealthBar, HealthBarTexture);
    }
}
