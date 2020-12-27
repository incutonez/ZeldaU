using UnityEngine;

public class WorldMatter : MonoBehaviour
{
    public Matter Matter { get; set; }
    public RectTransform HiddenDoor { get; set; }
    public SceneViewModel Transition { get; set; }

    private SpriteRenderer Renderer { get; set; }
    private WorldObjectData WorldObjectData { get; set; }
    private PolygonCollider2D Collider { get; set; }

    public void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        WorldObjectData = GetComponent<WorldObjectData>();
        Collider = GetComponent<PolygonCollider2D>();
    }

    public void SetMatter(Matter matter)
    {
        Matter = matter;
        Renderer.color = matter.GetColor();
        if (IsTransition())
        {
            // If we're in a transition, we have to set a certain layer that interacts with the player
            gameObject.layer = LayerMask.NameToLayer("Transition");
            Collider.isTrigger = true;
        }
        else
        {
            Renderer.sprite = matter.GetSprite();
            WorldObjectData.UpdatePolygonCollider2D();
            // If the player can enter this object, we need to change the collider's offset
            if (matter.CanEnter())
            {
                // TODOJEF: There's got to be a better way of doing this for the blocking layer that we have to add
                // when the player goes down stairs... we need all of this, so the character sprite appears behind
                // the floor, as if they're going down
                gameObject.layer = LayerMask.NameToLayer("Transition");
                Collider.offset = new Vector2(0, 1f);
                Collider.isTrigger = true;
                HiddenDoor = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}DoorBlock"));
                HiddenDoor.SetParent(GetComponent<RectTransform>());
                HiddenDoor.localPosition = new Vector3(0.5f, -1.75f);
            }
        }
    }

    public bool CanEnter()
    {
        return Matter.CanEnter();
    }

    public bool IsTransition()
    {
        return Matter.IsTransition();
    }

    public float GetPositionX()
    {
        return transform.localPosition.x;
    }

    public float GetPositionY()
    {
        return transform.localPosition.y;
    }

    public string GetSpriteName()
    {
        return Matter.type.GetDescription();
    }

    public Matter GetMatter()
    {
        return Matter;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
