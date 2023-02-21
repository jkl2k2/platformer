using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public new Camera camera;
    public TextMeshProUGUI coinText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray castToMouse = camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(castToMouse, out var hit)) {
                GameObject other = hit.collider.gameObject;
                if (other.name == "Brick(Clone)")
                {
                    other.GetComponent<ParticleSystem>().Play();
                    other.GetComponent<MeshRenderer>().enabled = false;
                    Destroy(other, 2f);
                } else if (other.name == "Question(Clone)")
                {
                    if (int.Parse(coinText.text) < 9)
                    {
                        coinText.SetText("0" + (int.Parse(coinText.text) + 1));
                    }
                    else
                    {
                        coinText.SetText("" + (int.Parse(coinText.text) + 1));
                    }
                }
            }
        }
    }
}
