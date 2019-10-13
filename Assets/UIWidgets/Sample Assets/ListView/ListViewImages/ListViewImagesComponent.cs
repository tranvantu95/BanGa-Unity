using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections.Generic;
using System.Collections;

namespace UIWidgetsSamples {
	public class ListViewImagesComponent : ListViewItem {
		[SerializeField]
		public Text Url;

		[SerializeField]
		public RawImage Image;

		[SerializeField]
		public bool isCycleImage ;

		[SerializeField]
		protected LayoutElement ImageLayoutElement;

		protected ListViewImagesItem Item;

		protected static Dictionary<string,Texture2D> Cache = new Dictionary<string, Texture2D>();

		protected bool IsLoading;

		protected IEnumerator loadCorutine;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (IsLoading)
			{
				return;
			}
			if ((Image.texture==null) && (Item!=null) && (Item.Url!=""))
			{
				loadCorutine = Load();
				StartCoroutine(loadCorutine);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (IsLoading)
			{
				IsLoading = false;
				StopCoroutine(loadCorutine);
			}
		}

		// Displaying item data
		public void SetData(ListViewImagesItem item)
		{
			// save item so later can fix item.Height to actual value
			Item = item;

			//			Url.text = (Item.Url!="") ? Item.Url : "No image";

			if (Cache.ContainsKey(Item.Url))
			{
				SetImage();
			}
			else
			{
				// reset images parameter
				ImageLayoutElement.preferredHeight = -1;
				ImageLayoutElement.preferredWidth = -1;

				if ((Item.Url!="") && (Item.Url!=null))
				{
					Image.color = Color.white;
					ImageLayoutElement.minHeight = item.Height;
					ImageLayoutElement.minWidth = item.Height;

					loadCorutine = Load();
					StartCoroutine(loadCorutine);
					Image.GetComponent<RectTransform>().sizeDelta = new Vector2( item.Height, item.Height);

				}
				else
				{
					Image.color = Color.clear;
					ImageLayoutElement.minHeight = -1;
					ImageLayoutElement.minWidth = -1;
				}
			}
		}

		void SetImage()
		{
			Image.color = Color.white;
			ImageLayoutElement.preferredHeight = Item.Height;
			ImageLayoutElement.preferredWidth = Item.Height;
			Image.GetComponent<RectTransform>().sizeDelta = new Vector2( Item.Height, Item.Height);

			if (isCycleImage) {
				
				//				texture.Resize (temd2, temd2);
				//			renderer.texture =  www.texture;
//				BeardedManStudios.Threading.Task.Run (() => {
					Texture2D texture = (Texture2D) Cache[Item.Url]; // also take any texture here
					int temd2 = texture.height;
					if (temd2 > texture.width)
						temd2 = texture.width;
					Texture2D temp = CalculateTexture (temd2, temd2, temd2 / 2, temd2 / 2, temd2 / 2,(Texture2D) Cache[Item.Url]);
					Image.texture = temp;
//				});

				//				Texture2D temp = CalculateTexture (texture.height, texture.width, texture.height / 2, texture.height / 2, texture.width / 2, texture);
				//				Image.texture = temp;
			} else {
				Image.texture = Cache[Item.Url];

			}
		}
		Texture2D CalculateTexture (int h, int w, float r, float cx, float cy, Texture2D sourceTex)
		{
			Color [] c = sourceTex.GetPixels (0, 0, sourceTex.width, sourceTex.height);
			Texture2D b = new Texture2D (h, w);
			for (int i = 0; i<(h*w); i++)
			{
				int y = Mathf.FloorToInt (((float)i) / ((float)w));
				int x = Mathf.FloorToInt (((float)i - ((float)(y * w))));
				if (r * r >= (x - cx) * (x - cx) + (y - cy) * (y - cy)) {
					b.SetPixel (x, y, c [i]);
				} else {
					b.SetPixel (x, y, Color.clear);
				}
			}
			b.Apply ();
			return b;
		}

		IEnumerator Load()
		{
			IsLoading = true;

			var url = Item.Url;

			yield return null;

			var www = new WWW(url);

			yield return www;
			if (!Cache.ContainsKey(url))
			{
				Cache.Add(url, www.texture);
			}
			if (Cache.ContainsKey(Item.Url))
			{
				SetImage();
			}

			IsLoading = false;
		}
	}

}