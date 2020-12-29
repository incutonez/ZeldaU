using UnityEngine;

public class WorldDoor : MonoBehaviour
{
    public RectTransform HiddenDoor { get; set; }

    private void Awake()
    {
        //HiddenDoor = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}DoorBlock"));
        //HiddenDoor.SetParent(transform);
        //HiddenDoor.localPosition = new Vector3(0.5f, -1.75f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHandler.IsTransitioning)
        {
            return;
        }
        WorldPlayerTransition player = collision.gameObject.GetComponent<WorldPlayerTransition>();
        if (player != null)
        {
            //HiddenDoor.gameObject.SetActive(true);
            // TODOJEF: Fix this... need to pass in transition
            StartCoroutine(GameHandler.SceneBuilder.EnterDoor(new WorldMatter()));
        }
    }
}
