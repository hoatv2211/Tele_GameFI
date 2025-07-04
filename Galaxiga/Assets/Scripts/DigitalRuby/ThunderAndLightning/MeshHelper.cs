using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class MeshHelper
	{
		public MeshHelper(Mesh mesh)
		{
			this.mesh = mesh;
			this.triangles = mesh.triangles;
			this.vertices = mesh.vertices;
			this.normals = mesh.normals;
			this.CalculateNormalizedAreaWeights();
		}

		public void GenerateRandomPoint(ref RaycastHit hit, out int triangleIndex)
		{
			triangleIndex = this.SelectRandomTriangle();
			this.GetRaycastFromTriangleIndex(triangleIndex, ref hit);
		}

		public void GetRaycastFromTriangleIndex(int triangleIndex, ref RaycastHit hit)
		{
			Vector3 barycentricCoordinate = this.GenerateRandomBarycentricCoordinates();
			Vector3 a = this.vertices[this.triangles[triangleIndex]];
			Vector3 vector = this.vertices[this.triangles[triangleIndex + 1]];
			Vector3 a2 = this.vertices[this.triangles[triangleIndex + 2]];
			hit.barycentricCoordinate = barycentricCoordinate;
			hit.point = a * barycentricCoordinate.x + vector * barycentricCoordinate.y + a2 * barycentricCoordinate.z;
			if (this.normals == null)
			{
				hit.normal = Vector3.Cross(a2 - vector, a - vector).normalized;
			}
			else
			{
				a = this.normals[this.triangles[triangleIndex]];
				vector = this.normals[this.triangles[triangleIndex + 1]];
				a2 = this.normals[this.triangles[triangleIndex + 2]];
				hit.normal = a * barycentricCoordinate.x + vector * barycentricCoordinate.y + a2 * barycentricCoordinate.z;
			}
		}

		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		public int[] Triangles
		{
			get
			{
				return this.triangles;
			}
		}

		public Vector3[] Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		public Vector3[] Normals
		{
			get
			{
				return this.normals;
			}
		}

		private float[] CalculateSurfaceAreas(out float totalSurfaceArea)
		{
			int num = 0;
			totalSurfaceArea = 0f;
			float[] array = new float[this.triangles.Length / 3];
			for (int i = 0; i < this.triangles.Length; i += 3)
			{
				Vector3 a = this.vertices[this.triangles[i]];
				Vector3 vector = this.vertices[this.triangles[i + 1]];
				Vector3 b = this.vertices[this.triangles[i + 2]];
				float sqrMagnitude = (a - vector).sqrMagnitude;
				float sqrMagnitude2 = (a - b).sqrMagnitude;
				float sqrMagnitude3 = (vector - b).sqrMagnitude;
				float x = (2f * sqrMagnitude * sqrMagnitude2 + 2f * sqrMagnitude2 * sqrMagnitude3 + 2f * sqrMagnitude3 * sqrMagnitude - sqrMagnitude * sqrMagnitude - sqrMagnitude2 * sqrMagnitude2 - sqrMagnitude3 * sqrMagnitude3) / 16f;
				float num2 = PathGenerator.SquareRoot(x);
				array[num++] = num2;
				totalSurfaceArea += num2;
			}
			return array;
		}

		private void CalculateNormalizedAreaWeights()
		{
			float num;
			this.normalizedAreaWeights = this.CalculateSurfaceAreas(out num);
			if (this.normalizedAreaWeights.Length == 0)
			{
				return;
			}
			float num2 = 0f;
			for (int i = 0; i < this.normalizedAreaWeights.Length; i++)
			{
				float num3 = this.normalizedAreaWeights[i] / num;
				this.normalizedAreaWeights[i] = num2;
				num2 += num3;
			}
		}

		private int SelectRandomTriangle()
		{
			float value = UnityEngine.Random.value;
			int i = 0;
			int num = this.normalizedAreaWeights.Length - 1;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this.normalizedAreaWeights[num2] < value)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i * 3;
		}

		private Vector3 GenerateRandomBarycentricCoordinates()
		{
			Vector3 a = new Vector3(UnityEngine.Random.Range(Mathf.Epsilon, 1f), UnityEngine.Random.Range(Mathf.Epsilon, 1f), UnityEngine.Random.Range(Mathf.Epsilon, 1f));
			return a / (a.x + a.y + a.z);
		}

		private Mesh mesh;

		private int[] triangles;

		private Vector3[] vertices;

		private Vector3[] normals;

		private float[] normalizedAreaWeights;
	}
}
