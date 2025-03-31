using UnityEngine;

public class GradientTextureGenerator : MonoBehaviour
{
    [Header("Gradient Settings")]
    public Gradient gradient;
    public int textureWidth = 256;
    public string textureName = "HealthBarGradient";

    [ContextMenu("Generate Gradient Texture")]
    public void GenerateGradientTexture()
    {
        Texture2D texture = new Texture2D(textureWidth, 1);
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < textureWidth; x++)
        {
            float t = (float)x / (textureWidth - 1);
            Color color = gradient.Evaluate(t);
            texture.SetPixel(x, 0, color);
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string path = Application.dataPath + "/" + textureName + ".png";
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log("Texture saved to: " + path);
    }
}
