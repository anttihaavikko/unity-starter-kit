using UnityEngine;

namespace AnttiStarterKit.Visuals
{
	[ExecuteInEditMode]
	public class LimbRenderer : MonoBehaviour {

		public bool bezier = false;
		public Transform[] bones;
		public LineRenderer[] lines;
		public float lineZ = 0f;

		private void LateUpdate ()
		{
			foreach (var line in lines)
			{
				line.SetPosition (0, new Vector3(transform.position.x, transform.position.y, lineZ));

				if (bezier) {
					for (var i = 1; i < line.positionCount; i++) {
						// B(t) = (1-t)^2P0 + 2(1-t)tP1 + t2P2 , 0 < t < 1
						var t = (float)i / (float)line.positionCount;
						var p = Mathf.Pow (1 - t, 2) * transform.position + 2 * (1 - t) * t * bones[0].position + Mathf.Pow (t, 2) * bones[1].position;
						p.z = 0.1f;
						line.SetPosition (i, p);
					}
				} else {
					for (var i = 0; i < bones.Length; i++) {
						line.SetPosition (i + 1, new Vector3(bones[i].position.x, bones[i].position.y, lineZ));
					}
				}
			}
		}
	}
}
