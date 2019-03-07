using UnityEngine;
using System.Collections;

public class bg_scroll : MonoBehaviour
{
    public GameObject bg;
    public GameObject player;
    public float mult;
    protected Material textureToAnimate;

    protected Vector2 uvOffset = Vector2.zero;
    public Vector2 uvAnimationRate = new Vector2(0.0f, 0.0f);
    public string textureName = "_MainTex";

    protected MeshRenderer backgroundMeshRenderer;

    [SerializeField]
    protected bool resetPositionToZero = true;

    protected void Start()
    {
        backgroundMeshRenderer = bg.GetComponent<MeshRenderer>();

        if (backgroundMeshRenderer != null)
        {
            if (resetPositionToZero)
                backgroundMeshRenderer.transform.position = Vector3.zero;

            textureToAnimate = backgroundMeshRenderer.material;
        }
    }

    protected void FixedUpdate()
    {
        float ymove = player.GetComponent<player_controller>().ymove;
        uvAnimationRate.y = -ymove;
        if (textureToAnimate != null)
        {
            if (uvOffset.x >= 1.0f)
            {
                uvOffset.x = 0.0f;
            }

            if (uvOffset.y >= 1.0f)
            {
                uvOffset.y = 0.0f;
            }

            uvOffset += uvAnimationRate * mult;
            textureToAnimate.mainTextureOffset = uvOffset;
        }
    }
}