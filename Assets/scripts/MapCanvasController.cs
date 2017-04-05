using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvasController : MonoBehaviour {

    public Canvas mapCanvas;
    public GameObject map;
    public GameObject mako;
    public GameObject finishArea;
    public GameObject finishMapIcon;
    public GameObject makoMapIcon;
    public GameObject terrain;

    private Vector3 irlToMapScale;
    private bool alreadyHandledThisToggle;

	void Start()
    {
        mapCanvas.enabled = false;
        alreadyHandledThisToggle = false;
        setIrlToMapScale();
        setFinishLocation();
    }

	void Update()
    {
        toggleMap();
        updateMakoLocation();
	}

    void setIrlToMapScale()
    {
        Vector3 terrainDimensions = terrain.GetComponent<Terrain>().terrainData.size;
        RectTransform canvasRT = map.GetComponent<RectTransform>();
        irlToMapScale = new Vector3(
            canvasRT.rect.width / terrainDimensions.x,
            0.0f,
            canvasRT.rect.width / terrainDimensions.z
        );
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
        finishMapIcon.transform.localPosition = mapPosition;
    }

    void updateMakoLocation()
    {
        Vector3 mapPosition = getMapPosition(mako.transform.position);
        makoMapIcon.transform.localPosition = mapPosition;
    }

    Vector3 getMapPosition(Vector3 irlPosition)
    {
        Vector3 mapPosition;
        mapPosition.x = irlPosition.x * irlToMapScale.x;

        mapPosition.y = irlPosition.z * irlToMapScale.z;
        mapPosition.z = 0.0f;
        return mapPosition;
    }
}
