using UnityEngine;

public class MeshTesting : MonoBehaviour
{
    public Mesh CreateEmptyMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[0];
        mesh.uv = new Vector2[0];
        mesh.triangles = new int[0];
        return mesh;
    }

    public void CreateEmptyMesh(int quadCount, out Vector3[] verts, out Vector2[] uvs, out int[] triangles)
    {
        verts = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    private void Start()
    {
        /* Mesh: The shape of your object... if it's a quad, then it's composed of 2 triangles
         * Mesh Renderer: This holds the material that you'd like to use... a material has a texture associated to it
         * Mesh Filter: Representation of the mesh data in the UI */
        Mesh mesh = new Mesh();
        int width = 4;
        int height = 4;
        // This is where the lines meet, or rather known as the node... note that these all line up, so
        // whatever's at the origin of vert 0 should also be the texture at uv 0 and triangle 0's starting point
        Vector3[] verts = new Vector3[4 * width * height];
        // This
        Vector2[] uvs = new Vector2[4 * width * height];
        // Triangles connect each vertex... so if triangle 0 is index 0, and triangle 1 is index 1, that's where
        // the first line is drawn, and so on... these should always be set in a clockwise motion
        int[] triangles = new int[6 * width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                verts[index * 4] = new Vector3(i, j);
                verts[index * 4 + 1] = new Vector3(i, j + 1);
                verts[index * 4 + 2] = new Vector3(i + 1, j + 1);
                verts[index * 4 + 3] = new Vector3(i + 1, j);

                uvs[index * 4] = Vector2.zero;
                uvs[index * 4 + 1] = new Vector2(0, 1);
                uvs[index * 4 + 2] = new Vector2(1, 1);
                uvs[index * 4 + 3] = new Vector2(1, 0);

                // Start at origin
                triangles[index * 6] = index * 4;
                // Go to top left
                triangles[index * 6 + 1] = index * 4 + 1;
                // Go to top right
                triangles[index * 6 + 2] = index * 4 + 2;
                // Go back to origin
                triangles[index * 6 + 3] = index * 4;
                // Go to top right
                triangles[index * 6 + 4] = index * 4 + 2;
                // Go to bottom right
                triangles[index * 6 + 5] = index * 4 + 3;
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
