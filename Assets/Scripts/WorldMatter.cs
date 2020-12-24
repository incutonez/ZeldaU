using UnityEngine;

public class WorldMatter : MonoBehaviour
{
    public Matter matter;
    public RectTransform hiddenDoor;
    public SceneViewModel transition;

    private new SpriteRenderer renderer;
    private WorldObjectData worldObjectData;

    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public void SetMatter(Matter matter)
    {
        this.matter = matter;
        renderer.color = matter.GetColor();
        if (IsTransition())
        {
            // If we're in a transition, we have to set a certain layer that interacts with the player
            gameObject.layer = LayerMask.NameToLayer("Transition");
        }
        else
        {
            renderer.sprite = matter.GetSprite();
            worldObjectData.UpdatePolygonCollider2D();
            // If the player can enter this object, we need to change the collider's offset
            if (matter.CanEnter())
            {
                // TODOJEF: There's got to be a better way of doing this for the blocking layer that we have to add
                // when the player goes down stairs... we need all of this, so the character sprite appears behind
                // the floor, as if they're going down
                gameObject.layer = LayerMask.NameToLayer("Transition");
                GetComponent<PolygonCollider2D>().offset = new Vector2(0, 1f);
                hiddenDoor = Instantiate(Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}DoorBlock"));
                hiddenDoor.SetParent(GetComponent<RectTransform>());
                hiddenDoor.localPosition = new Vector3(0.5f, -1.75f);
            }
        }
    }

    public bool CanEnter()
    {
        return matter.CanEnter();
    }

    public bool IsTransition()
    {
        return matter.IsTransition();
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
        return matter.type.GetDescription();
    }

    public Matter GetMatter()
    {
        return matter;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
