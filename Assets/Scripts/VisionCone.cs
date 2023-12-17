using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public LayerMask VisionObstructingLayer;
    public int VisionConeResolution = 120;
    [SerializeField] DifficultyGenerator difficultyGenerator;
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;
    MeshRenderer meshRenderer;

    string currentSceneName;
    string difficultyLevel;
    float difficultyFOV;
    float difficultyViewDistance;

    void Start()
    {
        gameObject.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();

        currentSceneName = SceneManager.GetActiveScene().name;
        foreach (DifficultyGenerator.Levels levels in difficultyGenerator.myLevels)
        {
            if (currentSceneName == levels.levelName)
            {
                difficultyLevel = levels.levelName;
                difficultyFOV = levels.enemyFOVAngle;
                difficultyViewDistance = levels.enemyViewDistance;
            }
        }
        difficultyFOV *= Mathf.Deg2Rad;

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -difficultyFOV / 2;
        float angleIcrement = difficultyFOV / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, difficultyViewDistance, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * difficultyViewDistance;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }
}
