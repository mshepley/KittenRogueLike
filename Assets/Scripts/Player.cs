using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int energyPerBird = 20;
	public int energyPerYarn = 10;
	public float restartLevelDelay = 1f;

	private Animator animator;
	private int energy;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();

		energy = GameManager.instance.playerEnergyPoints;

		base.Start ();
	}

	private void OnDisable()
	{
		GameManager.instance.playerEnergyPoints = energy;
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		energy--;

		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver ();

		GameManager.instance.playersTurn = false;
	}

	private void CheckIfGameOver()
	{
		if (energy <= 0)
			GameManager.instance.GameOver ();
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn)
			return;


		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall> (horizontal, vertical);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Bird") {
			energy += energyPerBird;
			other.gameObject.SetActive(false);
		}else if (other.tag == "Yarn") {
			energy += energyPerYarn;
			other.gameObject.SetActive(false);
		}
	}

	protected override void OnCantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerHit");

	}

	private void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);

	}

	public void LoseEnergy(int loss)
	{
		animator.SetTrigger ("playerHit");
		energy -= loss;
		CheckIfGameOver ();

	}

}
