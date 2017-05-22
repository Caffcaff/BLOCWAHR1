using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFactory : MonoBehaviour {

	public int playerID;
	public List<GameObject> myFactories = new List<GameObject>();

	public List<FactoryOrder> orderQueue = new List<FactoryOrder> ();

	public float interval = 1;
	public float f_interval;


	void OnEnable(){
		eventManager.onBuildInit += reCache;
		eventManager.onAIFactoryOrder += onFactoryOrder;
	}

	void OnDisable(){

		eventManager.onBuildInit -= reCache;
		eventManager.onAIFactoryOrder += onFactoryOrder;

	}



	// Use this for initialization
	void Start () {

		playerID = GetComponent<AICommand> ().playerID;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (orderQueue.Count > 0) {

			f_interval -= Time.deltaTime;

			if (f_interval < 0) {
				f_interval = interval;
				cycleOrder ();

			}
		}
	}

	void cycleOrder(){

		if (orderQueue.Count > 0 && myFactories.Count > 0) {

			FactoryOrder currOrder = orderQueue [0];
			List<GameObject> CandidateFactories = new List<GameObject> ();

			foreach (GameObject factory in myFactories) {

				if (factory.GetComponent<factoryManager> ().techLevel >= currOrder.tLevel) {
					CandidateFactories.Add (factory);
				}
			}
			if (CandidateFactories.Count == 0) {
				Debug.Log ("Player " + playerID + ": no factories with required Tech level");
				orderQueue.RemoveAt (0);
				orderQueue.Add (currOrder);
				return;
			} else {
				int F = Random.Range (0, CandidateFactories.Count);
				currOrder.ofactory = CandidateFactories [F];
				eventManager.ServeFactoryOrder(currOrder);
				orderQueue.RemoveAt (0);
				Debug.Log ("Factory Order Served");

			}









		}




	// currOrder = orderQueue[0]

	// if cash is enough

	// submit order to factory

	//else move to end of list.

}

	void onFactoryOrder(FactoryOrder order){

		int i = 0;

		while(i < order.miner){
			FactoryOrder unitOrder = new FactoryOrder (playerID, order.tLevel, 1, 0, 0, 0);
			orderQueue.Add (unitOrder);
			i++;
				}

		i = 0;

		while(i < order.mgun){
			FactoryOrder unitOrder = new FactoryOrder (playerID, order.tLevel, 0, 1, 0, 0);
			orderQueue.Add (unitOrder);
			i++;
		}

		i = 0;

		while(i < order.missile){
			FactoryOrder unitOrder = new FactoryOrder (playerID, order.tLevel, 0, 0, 1, 0);
			orderQueue.Add (unitOrder);
			i++;
		}

		i = 0;

		while(i < order.utility){
			FactoryOrder unitOrder = new FactoryOrder (playerID, order.tLevel, 0, 0, 0, 1);
			orderQueue.Add (unitOrder);
			i++;
		}

	}

	void reCache(Vector3 position, int type) {

		myFactories.Clear ();

		List<GameObject> allStructures = new List<GameObject> ();
		allStructures.AddRange (GameObject.FindGameObjectsWithTag ("Structure"));

		foreach (GameObject part in allStructures) {

			if(part.GetComponent<factoryManager>() != null && part.GetComponent<buildLogic>().playerID == playerID){

				myFactories.Add (part);

			}
		}

	}

}

