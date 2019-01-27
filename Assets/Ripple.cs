using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Ripple : MonoBehaviour
{
	public Shader shader;
	public float radius;
	public Texture2D mod_source;
	private Material m_Material;
	private int time;

	protected Material material {
		get {
			if (m_Material == null) {
				if (shader == null) {
					shader = Shader.Find ("ripple");
				}
				m_Material = new Material (shader);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_Material;
		} 
	}

	protected virtual void Start() {
		time = 0;
		radius = 0;
		if (shader == null) {
			shader = Shader.Find ("ripple");
		}
		m_Material = new Material(shader);
	}

	void Update () {
		time++;
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("time", time);
		material.SetFloat("radius", radius);
		material.SetTexture ("ModTex", mod_source);
		Graphics.Blit(source, destination, m_Material);
	}
		

	protected virtual void OnDisable() {
		if( m_Material ) {
			DestroyImmediate( m_Material );
		}
	}

	[System.Obsolete("Use Graphics.Blit(source,dest) instead")]
	public static void Blit(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}

	[System.Obsolete("Use Graphics.Blit(source, destination, material) instead")]
	public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest, material);
	}  
}

