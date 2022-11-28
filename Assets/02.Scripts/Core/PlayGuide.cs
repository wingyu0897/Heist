using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dec
{
    public Sprite sprite;
    public string text;
}

public class PlayGuide : MonoBehaviour
{
    public List<Dec> decs;
	public Image img;
	public TextMeshProUGUI txt;
    public Button nextBtn;
    public Button beforeBtn;
    public int currDec = 0;

	private void Start()
	{
		if (decs.Count > 0)
		{
			img.sprite = decs[0].sprite;
			txt.text = decs[0].text;
		}
		beforeBtn.gameObject.SetActive(false);
	}

	public void ShowDecs(int dir)
	{
		if (decs.Count > 0)
		{
			if (dir < 0)
			{
				if (currDec == 0)
				{
					return;
				}
				else
				{
					currDec-=1;
					nextBtn.gameObject.SetActive(true);
				}
				img.sprite = decs[currDec].sprite;
				txt.text = decs[currDec].text;
				if (currDec == 0)
				{
					beforeBtn.gameObject.SetActive(false);
				}
				else
				{
					beforeBtn.gameObject.SetActive(true);
				}
			}
			if (dir > 0)
			{
				if (currDec >= decs.Count - 1)
				{
					return;
				}
				else
				{
					currDec+=1;
					beforeBtn.gameObject.SetActive(true);
				}
				img.sprite = decs[currDec].sprite;
				txt.text = decs[currDec].text;
				if (currDec >= decs.Count - 1)
				{
					nextBtn.gameObject.SetActive(false);
				}
				else
				{
					nextBtn.gameObject.SetActive(true);
				}
			}

		}
	}
}
