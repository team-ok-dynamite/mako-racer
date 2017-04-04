using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvasController : MonoBehaviour {

    public Canvas mapCanvas;
    public GameObject mako;
    public GameObject finishArea;
    public GameObject finishMapIcon;
    public GameObject makoMapIcon;

    private bool alreadyHandledThisToggle;

	void Start()
    {
        mapCanvas.enabled = false;
        alreadyHandledThisToggle = false;
        setFinishLocation();
    }

	void Update()
    {
        toggleMap();
        updateMakoLocation();
	}

    void toggleMap()
    {
        bool mapKeyDown = Input.GetButtonDown("Map");
        if (!mapKeyDown)
        {
            alreadyHandledThisToggle = false;
            return;
        }
        else if (alreadyHandledThisToggle)
        {
            return;
        }
        else if (mapKeyDown)
        {
            mapCanvas.enabled = !mapCanvas.enabled;
            alreadyHandledThisToggle = true;
        }
    }

    void setFinishLocation()
    {
        Vector3 mapPosition = getMapPosition(finishArea.transform.position);
        finishMapIcon.transform.Translate(mapPosition);
    }

    void updateMakoLocation()
    {
        Vector3 mapPosition = getMapPosition(mako.transform.position);
        makoMapIcon.transform.localPosition = mapPosition;
    }

    Vector3 getMapPosition(Vector3 irlPosition)
    {
        const float irlToMapScale = 2.5f;
        Vector3 mapPosition;
        mapPosition.x = irlPosition.x / irlToMapScale;
        mapPosition.y = irlPosition.z / irlToMapScale;
        mapPosition.z = 0;
        return mapPosition;
    }
}
