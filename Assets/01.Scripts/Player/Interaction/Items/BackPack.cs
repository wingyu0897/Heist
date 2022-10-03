using UnityEngine;

public class BackPack : MonoBehaviour, IInteractable
{
	[SerializeField] private float interactionTime;
	public float InteractionTime => interactionTime;
	private bool canInteractive = true;
	public bool CanInteractive { get => canInteractive; set => canInteractive = value; }
	public GameObject Objectgame => gameObject;
	private Transform backPackHolder;

	private void Start()
	{
		backPackHolder = GameObject.Find("BackPackHolder").transform;
	}

	private void Update()
	{
		canInteractive = Interactable();
		if (transform.parent != null && Input.GetKeyDown(KeyCode.G))
		{
			Drop();
		}
	}

	private void Drop()
	{
		PlayerData.Instance.backPacks -= 1;
		transform.parent = null;
		transform.position = transform.localPosition - new Vector3(0, 0.5f, 0);
	}

	private bool Interactable()
	{
		return transform.parent != null ? false : PlayerData.Instance.backPacks == 0 ? true : false;
	}

	#region IIteractable
	public void Action()
	{
		PlayerData.Instance.backPacks += 1;
		transform.SetParent(backPackHolder);
		transform.localPosition = Vector3.zero;
	}

	public void Init()
	{
		canInteractive = true;
		transform.parent = null;
	}
	#endregion
}
