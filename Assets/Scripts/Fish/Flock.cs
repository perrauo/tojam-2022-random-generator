﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tojam2022
{
	public class Flock : MonoBehaviour
	{

		public FlockManager myManager;
		float speed;
		float speedMultiplier;


		bool turning = false;
		Animator anim;

        // Use this for initialization
        void Start()
		{
			speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
			//anim = gameObject.GetComponent<Animator>();
			//anim.SetFloat("cycleOffset", Random.Range(0.0f, 1.0f));
			//anim.SetFloat("speedMult", speed);
		}

		// Update is called once per frame
		void Update()
		{
			//determine the bounding box of the manager cube
			Bounds b = new Bounds(myManager.transform.position, myManager._SwimLimits * 2);
			// if fish is outside the bounds of the cube or about to hit something
			// then start turning around
			RaycastHit hit = new RaycastHit();

			//turn towards the centre of the manager cube
			Vector3 direction = Vector3.zero;


			if (!b.Contains(transform.position))
			{
				turning = true;
				direction = myManager.transform.position - transform.position;
			}
			else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
			{
				turning = true;
				direction = Vector3.Reflect(this.transform.forward, hit.normal);
			}
			else
			{
				turning = false;
			}

			if (turning)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
										Quaternion.LookRotation(direction),
										myManager.rotationSpeed * Time.deltaTime);
			}
			else
			{
				if (Random.Range(0, 100) < 10)
				{
					speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
				}
				if (Random.Range(0, 100) < 20)
				{
					ApplyRules();
				}
				//anim.SetFloat("speedMult", speed);
			}
			transform.Translate(0, 0, Time.deltaTime * speed);
		}

		void ApplyRules()
		{
			GameObject[] gos;
			gos = myManager._AllFish;

			Vector3 vcentre = Vector3.zero;
			Vector3 vavoid = Vector3.zero;
			float gSpeed = 0.01f;
			float nDistance;
			int groupSize = 0;

			foreach (GameObject go in gos)
			{
				if (go != this.gameObject)
				{
					nDistance = Vector3.Distance(go.transform.position, this.transform.position);
					if (nDistance <= myManager.neighbourDistance)
					{
						vcentre += go.transform.position;
						groupSize++;
						if (nDistance < 1.0f)
						{
							vavoid = vavoid + (this.transform.position - go.transform.position);
						}
						Flock anotherFlock = go.GetComponent<Flock>();
						gSpeed = gSpeed + anotherFlock.speed;
					}
				}
			}
			if (groupSize > 0)
			{
				vcentre = vcentre / groupSize + (myManager._GoalPos - this.transform.position);
				speed = gSpeed / groupSize;

				Vector3 direction = (vcentre + vavoid) - transform.position;
				if (direction != Vector3.zero)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation,
															Quaternion.LookRotation(direction),
															myManager.rotationSpeed * Time.deltaTime);
				}
			}
		}
	}
}
